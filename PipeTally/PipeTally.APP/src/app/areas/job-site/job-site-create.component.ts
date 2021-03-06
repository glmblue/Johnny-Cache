import { Component, ViewChild, OnInit } from "@angular/core";
import { Router } from "@angular/router";
import { MessageService } from "../../services/message.service";
import { BusyService } from "../../services/busy.service";
import { JobSiteService } from "../../services/job-site.service";
import { JobSiteModel } from "../../models/job-site-model";
import { JobSiteEditComponent } from "./shared/job-site-edit.component";

@Component({
  selector: "app-job-site-create",
  templateUrl: "./job-site-create.component.html"
})
export class JobSiteCreateComponent implements OnInit {
  constructor(
    private _router: Router,
    private _jobSiteService: JobSiteService,
    public messageSvc: MessageService,
    private _busySvc: BusyService
  ) { }

  public entity: JobSiteModel = new JobSiteModel();

  @ViewChild(JobSiteEditComponent)
  private _edit: JobSiteEditComponent;

  public Save() {
    if (!this._edit.ReadForm()) { return; }
    
    this._busySvc.SetBusy();
    this._jobSiteService.Post(this.entity)
      .subscribe(x => {
        this._busySvc.SetFree();
        this.messageSvc.AddSuccess("job site create", "The job site was created successfully.")
        this._router.navigate(["/job-site-read", x.JobSiteId]);
      },
      error => {
        this._busySvc.SetFree();
        this.messageSvc.AddError("job site create", error)
      });
  }

  public Reset() {
    this.entity = new JobSiteModel();
    this.messageSvc.AddSuccess("job site type create", "The job site type was reset.")
  }

  public CanCreate: boolean = false;

  ngOnInit() {
  }
}
