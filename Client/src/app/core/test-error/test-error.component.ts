import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-test-error',
  templateUrl: './test-error.component.html',
  styleUrls: ['./test-error.component.scss']
})
export class TestErrorComponent implements OnInit {

  baseUrl = environment.appUrl
  validationErrors:any


  constructor(private http:HttpClient) { }

  ngOnInit(): void {
  }

  get500Error(){
    this.http.get(this.baseUrl + 'buggy/servererror').subscribe(res =>{

    }, error =>{

    })

  }

  get404Error(){

    this.http.get(this.baseUrl + 'buggy/notfound').subscribe(res =>{

    }, error =>{

    })
  }

  get400Error(){
    this.http.get(this.baseUrl + 'buggy/badrequest').subscribe(res =>{

    }, error =>{
      
    })

  }
  
  get400ValidationError(){
    this.http.get(this.baseUrl + 'product/fortytwo').subscribe(res =>{
    },error =>{
      this.validationErrors = error
      console.log(this.validationErrors)
    })

  }

}
