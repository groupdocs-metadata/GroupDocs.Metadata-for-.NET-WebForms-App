<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="Metadata_Editor_Modren_UI._default" %>
<!DOCTYPE html>
<html lang="en">
   <head>
      <title>GroupDocs.Metadata for .NET (Web Forms)</title>
      <meta http-equiv="content-type" content="text/html; charset=utf-8" />
      <meta name="viewport" content="width=device-width, initial-scale=1">
      <link rel="stylesheet" href="https://ajax.googleapis.com/ajax/libs/angular_material/1.1.0/angular-material.min.css">
      <link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons|Roboto+Condensed:400,700">
      <link href="Content/style.css" rel="stylesheet" />
      <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.1/angular.min.js"></script>
      <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.1/angular-animate.min.js"></script>
      <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.1/angular-aria.min.js"></script>
      <script src="https://ajax.googleapis.com/ajax/libs/angularjs/1.6.1/angular-resource.min.js"></script>
      <script src="https://ajax.googleapis.com/ajax/libs/angular_material/1.1.0/angular-material.min.js"></script>
      <script src="Content/custom.js"></script>
      <script src="/Content/app.js"></script>
   </head>
   <body>
      <div ng-app="GroupDocsMetadata" ng-cloak flex layout="column" style="height: 100%;">
         <md-toolbar ng-controller="ToolbarController" layout="row" hide-print md-whiteframe="4" class="md-toolbar-tools md-scroll-shrink">
            <a href="/"><img src="/Content/GDVLogo.png" /></a>&nbsp; 
            <a href="/">
               <h1>Metadata for .NET (Web Forms)</h1>
            </a>
            <span flex></span>
            <md-button class="button" ng-href="/CleanMetadata?file={{ selectedFile }}" ng-click="cleamDocument()" ng-disabled="!selectedFile" ng-hide="ShowHideTools.ShowDownloads">
               <md-icon md-menu-origin md-menu-align-target>clear</md-icon>
               Clean Metadata
               <md-tooltip>Clean Metadata & Download Updated File</md-tooltip>
            </md-button>
            <md-button class="md-icon-button" ng-click="showTabDialog($event)" ng-hide="ShowHideTools.IsFileSelection">
               <md-icon md-menu-origin md-menu-align-target>library_books</md-icon>
               <md-tooltip>File Manager</md-tooltip>
            </md-button>
            <md-button ng-click="previousDocument()" class="md-icon-button" ng-disabled="!selectedFile" ng-hide="ShowHideTools.IsFileSelection">
               <md-icon>navigate_before</md-icon>
               <md-tooltip>Previous Document</md-tooltip>
            </md-button>
            <span>
               {{ selectedFile }}      
               <md-tooltip>Current Selected File</md-tooltip>
            </span>
            <md-button ng-click="nextDocument()" class="md-icon-button" ng-disabled="!selectedFile" ng-hide="ShowHideTools.IsFileSelection">
               <md-icon>navigate_next</md-icon>
               <md-tooltip>Next Document</md-tooltip>
            </md-button>
            <md-menu>
               <md-button class="md-icon-button" ng-click="this.openMenu($mdOpenMenu, $event)">
                  <md-icon>save</md-icon>
                  <md-tooltip>Export Metadata</md-tooltip>
               </md-button>
               <md-menu-content width="4">
                  <md-menu-item ng-hide="ShowHideTools.IsShowDownloads">
                     <md-button ng-href="/ExportMetadata?isExcel=true&file={{ selectedFile }}" ng-click="exportDocument()" target="_blank" ng-disabled="!selectedFile">
                        <md-icon md-menu-origin md-menu-align-target>file_download</md-icon>
                        Export Metadata to Excel
                     </md-button>
                  </md-menu-item>
                  <md-menu-item ng-hide="ShowHideTools.IsShowDownloads">
                     <md-button ng-href="/ExportMetadata?isExcel=false&file={{ selectedFile }}" ng-click="exportDocument()" target="_blank" ng-disabled="!selectedFile">
                        <md-icon md-menu-origin md-menu-align-target>file_download</md-icon>
                        Export Metadata to CSV
                     </md-button>
                  </md-menu-item>
               </md-menu-content>
            </md-menu>
            <md-button class="md-icon-button" ng-href="/downloadoriginal?file={{ selectedFile }}" ng-disabled="!selectedFile" ng-hide="ShowHideTools.ShowDownloads">
               <md-icon md-menu-origin md-menu-align-target>file_download</md-icon>
               <md-tooltip>Download Original File</md-tooltip>
            </md-button>
            <md-button class="md-icon-button">
               <md-icon>more_vert</md-icon>
            </md-button>
         </md-toolbar>
         <md-content flex layout="row" md-scroll-y>
            <md-content flex id="content" class="md-padding" role="main">
               <md-card>
                  <md-card-title-media>
                     <div class="md-media-lg card-media md-padding">
                        <h1>Metadata Details: {{ selectedFile }}</h1>
                        <div ng-controller="MetadataController">
                           <div class="md-media-lg card-media md-padding">
                              <table>
                                 <tr>
                                    <th style="text-align:center;">#</th>
                                    <th>Property</th>
                                    <th>Value</th>
                                 </tr>
                                 <tr ng-repeat="item in MetadataProperties | orderBy:'Name'" ng-value="item">
                                    <td align="center">{{$index + 1}}</td>
                                    <td><span class="fileLink">{{ item.Name }}</span></td>
                                    <td><input type="text" readonly value="{{ item.Value }}" /></td>
                                 </tr>
                              </table>
                           </div>
                        </div>
                     </div>
                  </md-card-title-media>
               </md-card>
            </md-content>
         </md-content>
         <div style="visibility: hidden">
            <div class="md-dialog-container" id="fuDialog">
               <md-dialog aria-label="File Manager">
                  <md-toolbar>
                     <div class="md-toolbar-tools">
                        <h2>
                           <md-icon md-menu-origin md-menu-align-target>library_books</md-icon>
                           File Manager
                        </h2>
                        <span flex></span>
                        <md-button class="md-icon-button" ng-click="cancel()">
                           <md-icon aria-label="Close dialog">close</md-icon>
                        </md-button>
                     </div>
                  </md-toolbar>
                  <md-dialog-content style="max-width:800px;max-height:810px; ">
                     <md-tabs md-dynamic-height md-border-bottom md-selected="tabselectedIndex">
                        <md-tab label="Upload">
                           <md-content class="md-padding" style="min-width:500px;">
                              <md-card>
                                 <md-card-title>
                                    <md-card-title-text>
                                       <span class="md-headline">
                                          <md-icon md-menu-origin md-menu-align-target>file_upload</md-icon>
                                          Upload
                                       </span>
                                    </md-card-title-text>
                                 </md-card-title>
                                 <md-card-title-media>
                                    <div ng-controller="ToolbarController">
                                       <div class="md-media-lg card-media md-padding">
                                          <input type="file" id="file" name="file" accept=".png,.gif,.jpeg,.bmp,.doc,.docx,.xls,.xlsx,.pdf,.dwg,.dxf,.eml,.msg" onchange="angular.element(this).scope().getFileDetails(this)" />
                                          <md-button class="md-raised md-primary" ng-click="uploadFiles()">
                                             <md-icon md-menu-origin md-menu-align-target>file_upload</md-icon>
                                             Upload
                                             <md-tooltip>Upload Selected File</md-tooltip>
                                          </md-button>
                                          <!--ADD A PROGRESS BAR ELEMENT.-->
                                          <p style="display: none" id="progress">
                                             <md-progress-linear md-mode="indeterminate" determinateValue="0" determinateValue2="0"></md-progress-linear>
                                          </p>
                                       </div>
                                    </div>
                                 </md-card-title-media>
                              </md-card>
                           </md-content>
                        </md-tab>
                        <md-tab label="Files">
                           <md-content class="md-padding" style="min-width:500px;">
                              <md-card>
                                 <md-card-title>
                                    <md-card-title-text>
                                       <span class="md-headline">
                                          <md-icon md-menu-origin md-menu-align-target>storage</md-icon>
                                          Files
                                       </span>
                                    </md-card-title-text>
                                 </md-card-title>
                                 <md-card-title-media>
                                    <div ng-controller="AvailableFilesController">
                                       <div class="md-media-lg card-media md-padding">
                                          <table id="filesList">
                                             <tr>
                                                <th align="center">#</th>
                                                <th>File Name</th>
                                             </tr>
                                             <tr ng-repeat="item in list" ng-value="item">
                                                <td align="center">{{$index + 1}}</td>
                                                <td><span class="fileLink" ng-click="onChange(item)">{{ item }}</span></td>
                                             </tr>
                                          </table>
                                       </div>
                                    </div>
                                 </md-card-title-media>
                              </md-card>
                           </md-content>
                        </md-tab>
                     </md-tabs>
                  </md-dialog-content>
               </md-dialog>
            </div>
         </div>
      </div>
   </body>
</html>