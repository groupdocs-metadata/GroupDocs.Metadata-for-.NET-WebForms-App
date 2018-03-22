'use strict';

var ngApp = angular.module('GroupDocsMetadata', ['ngMaterial', 'ngResource']);
ngApp.value('FilePath', DefaultFilePath);
ngApp.value('tabselectedIndex', 0);
ngApp.value('ShowHideTools', {
    IsFileSelection: !ShowFileSelection,
    IsShowDownloads: !ShowDownloads
});

ngApp.factory('FilesFactory', function ($resource) {
    return $resource('/files', {}, {
        query: {
            method: 'GET',
            isArray: true
        }
    });
});

ngApp.factory('MetadataPropertyFactory', function ($resource) {
    return $resource('/MetadataProperty?file=:filename', {}, {
        query: {
            method: 'GET',
            isArray: true
        }
    });
});

ngApp.controller('ToolbarController', function ToolbarController($rootScope, $sce, $scope, $mdSidenav, ShowHideTools, FilePath, $mdDialog, FilesFactory, tabselectedIndex, MetadataPropertyFactory) {
    $rootScope.tabselectedIndex = tabselectedIndex;
    $scope.showTabDialog = function (ev) {
        $mdDialog.show({
            controller: DialogController,
            contentElement: '#fuDialog',
            parent: angular.element(document.body),
            targetEvent: ev,
            clickOutsideToClose: true
        })
            .then(function (answer) {
                $scope.status = 'You said the information was "' + answer + '".';
            }, function () {
                $scope.status = 'You cancelled the dialog.';
            });
    };

    $scope.openMenu = function ($mdOpenMenu, ev) {
        $mdOpenMenu(ev);
    };

    function DialogController($scope, $mdDialog) {
        $scope.hide = function () {
            $mdDialog.hide();
        };
        $scope.cancel = function () {
            $mdDialog.cancel();
        };
        $scope.answer = function (answer) {
            $mdDialog.hide(answer);
        };
    };

    $scope.getFileDetails = function (e) {

        $scope.files = [];
        $scope.$apply(function () {
            for (var i = 0; i < e.files.length; i++) {
                $scope.files.push(e.files[i])
            }

        });
    };

    $scope.uploadFiles = function () {
        var data = new FormData();

        if ($scope.files != undefined) {

            for (var i in $scope.files) {
                data.append("uploadedFile", $scope.files[i]);
            }

            var objXhr = new XMLHttpRequest();
            objXhr.addEventListener("progress", updateProgress, false);
            objXhr.addEventListener("load", transferComplete, false);

            objXhr.open("POST", "/fileUpload/");
            objXhr.send(data);
            document.getElementById('progress').style.display = 'block';
            $scope.files = undefined;
        }
        else {
            alert("Please select file to upload.");
        }
    };

    function updateProgress(e) {
        if (e.lengthComputable) {
            document.getElementById('progress').style.display = 'block';
            document.getElementById('progress').setAttribute('max', e.total);
        }
    };

    function transferComplete(e) {
        $rootScope.list = FilesFactory.query();
        alert("Files uploaded successfully.");
        document.getElementById('progress').style.display = 'none';
        $rootScope.tabselectedIndex = 1;
    };

    $scope.uploadedFile = {};
    $scope.uploadedFile.name = "";

    $scope.ShowHideTools = {
        IsFileSelection: ShowHideTools.IsFileSelection,
        IsShowDownloads: ShowHideTools.IsShowDownloads
    };

    $scope.$on('selected-file-changed', function ($event, selectedFile) {
        $rootScope.selectedFile = selectedFile;
        DefaultFilePath = selectedFile;
    });

    $scope.nextDocument = function () {
        if ($rootScope.list.indexOf($rootScope.selectedFile) + 1 == $rootScope.list.length) {
            $rootScope.MetadataProperties = MetadataPropertyFactory.query({
                filename: $rootScope.list[0]
            });
            $rootScope.$broadcast('selected-file-changed', $rootScope.list[0]);
        }
        else {
            $rootScope.MetadataProperties = MetadataPropertyFactory.query({
                filename: $rootScope.list[$rootScope.list.indexOf($rootScope.selectedFile) + 1]
            });
            $rootScope.$broadcast('selected-file-changed', $rootScope.list[$rootScope.list.indexOf($rootScope.selectedFile) + 1]);
        }
    };

    $scope.previousDocument = function () {
        if ($rootScope.list.indexOf($rootScope.selectedFile) - 1 == -1) {
            $rootScope.MetadataProperties = MetadataPropertyFactory.query({
                filename: $rootScope.list[$rootScope.list.length - 1]
            });
            $rootScope.$broadcast('selected-file-changed', $rootScope.list[$rootScope.list.length - 1]);
        }
        else {
            $rootScope.MetadataProperties = MetadataPropertyFactory.query({
                filename: $rootScope.list[$rootScope.list.indexOf($rootScope.selectedFile) - 1]
            });
            $rootScope.$broadcast('selected-file-changed', $rootScope.list[$rootScope.list.indexOf($rootScope.selectedFile) - 1]);
        }
    };

    $scope.cleamDocument = function () {
        //$sce.trustAsResourceUrl('/CleanMetadata?file=' + $rootScope.selectedFile);
        //$rootScope.list = FilesFactory.query();
        //$rootScope.$broadcast('selected-file-changed', 'Cleaned_' + $rootScope.selectedFile);
    };

});


ngApp.controller('AvailableFilesController', function AvailableFilesController($rootScope, $scope, FilesFactory, FilePath, $mdDialog, MetadataPropertyFactory) {
    $rootScope.list = FilesFactory.query();

    $scope.onOpen = function () {
        $rootScope.list = FilesFactory.query();

    };
    $scope.onChange = function (item) {
        $mdDialog.hide();
        $rootScope.MetadataProperties = MetadataPropertyFactory.query({
            filename: item
        });

        $rootScope.$broadcast('selected-file-changed', item);
    };
});

ngApp.controller('MetadataController', function MetadataController($rootScope, $scope, MetadataPropertyFactory, FilePath, $mdDialog) {
    if (FilePath) {
        $rootScope.selectedFile = FilePath;
        $rootScope.MetadataProperties = MetadataPropertyFactory.query({
            filename: FilePath
        });
    }
});
