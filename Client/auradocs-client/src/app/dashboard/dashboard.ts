import { CommonModule } from '@angular/common';
import { AfterViewInit, Component, ElementRef, ViewChild } from '@angular/core';
import { Chart, registerables } from 'chart.js';
import { Chatbot } from '../components/chatbot/chatbot';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule,Chatbot],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.scss',
})
export class Dashboard implements AfterViewInit  {
  public quickActionsList:string[] = ["create/upload document","Start workflow","schedule meeting","View tutorial"];
  public tableHeadersList:string[] = ["Name","Last Updated","Owner"];
  public tableContentList:string[][] = [["test","2 hour ago","vivek"],["test1","yesterday","Ankit"],["test 2","3 days ago","team"],["test","2 hour ago","vivek"],["test1","yesterday","Ankit"],["test 2","3 days ago","team"]];
  @ViewChild('canvasSeven') canvasSeven!:ElementRef;
  @ViewChild('canvasThirty') canvasThirty!:ElementRef;
  constructor(){
    Chart.register(...registerables);
  }
  public onQuickActionClick(value:string){

  }

  public ngAfterViewInit(): void {
    const canvasSevenEle = this.canvasSeven.nativeElement as HTMLCanvasElement ;
    const sevendaysBarChart = new Chart(canvasSevenEle,
      {
        type:'bar',
        data:{
          labels: ['Product A', 'Product B', 'Product C', 'Product D'], //x-axis
          datasets:[{
            label:'',
            data: [120,90,150,70],//y-axis
            backgroundColor: [
                'rgba(75, 192, 192, 0.6)',
                'rgba(255, 99, 132, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(54, 162, 235, 0.6)'
            ],
            borderColor: [
                'rgba(75, 192, 192, 1)',
                'rgba(255, 99, 132, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(54, 162, 235, 1)'
            ],
            borderWidth: 1
          }]
        },
        options:{
          scales:{
            y:{
              beginAtZero:true
            }
          },
          plugins:{
            legend:{
              display:true,
              position:'top'
            },
            title:{
              display:true,
              text:'Activity (7 Days)'
            }
          }
        }
    });
    const canvasThirtyEle = this.canvasThirty.nativeElement as HTMLCanvasElement;
    const thirtydaysBarChart = new Chart(canvasThirtyEle,
      {
        type:'bar',
        data:{
          labels: ['Product A', 'Product B', 'Product C', 'Product D'], //x-axis
          datasets:[{
            label:'',
            data: [120,90,150,70],//y-axis
            backgroundColor: [
                'rgba(75, 192, 192, 0.6)',
                'rgba(255, 99, 132, 0.6)',
                'rgba(255, 206, 86, 0.6)',
                'rgba(54, 162, 235, 0.6)'
            ],
            borderColor: [
                'rgba(75, 192, 192, 1)',
                'rgba(255, 99, 132, 1)',
                'rgba(255, 206, 86, 1)',
                'rgba(54, 162, 235, 1)'
            ],
            borderWidth: 1
          }]
        },
        options:{
          scales:{
            y:{
              beginAtZero:true
            }
          },
          plugins:{
            legend:{
              display:true,
              position:'top'
            },
            title:{
              display:true,
              text:'Activity (30 Days)'
            }
          }
        }
    });
  }

}
