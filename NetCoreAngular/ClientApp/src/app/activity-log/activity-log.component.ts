//
import { Component, Inject, ViewChild, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
//
@Component({
  selector   :  'app-activity-log',
  templateUrl:  './activity-log.component.html',
  styleUrls  : ['./activity-log.component.css']
})
//
export class ActivityLogComponent implements OnInit {
  //
  public accessLogEntities: AccessLogEntity[];
  //
  columnas: string[]            = ['PageName', 'IpValue', 'AccessDate'];
  dataSource = null;
  @ViewChild(MatPaginator, { static: true }) paginator: MatPaginator;
  //
  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    //
    console.log("_baseUrl: " + baseUrl);
    //
    http.get<AccessLogEntity[]>
      (baseUrl + 'accessLog')
      .subscribe
      (
        result => {
          //
          this.accessLogEntities = result;
          //
          this.dataSource = new MatTableDataSource<AccessLogEntity>(this.accessLogEntities);
          this.dataSource.paginator = this.paginator;
          //
          console.log('SETTING_DATASOURCE ' + this.accessLogEntities.length);
        }
        , error => {
          //
          console.log(error)
        }
      );
   }
   //
   ngOnInit()
   {

   }
}
//
interface AccessLogEntity
{
  ID_column     : number;
  PageName      : string;
  AccessDate    : Date;
  IpValue       : string;
  LogType       : number;
}
