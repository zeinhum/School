import { TableRowDuplicate, FormSubmitter, RowDataCollector } from "./FormModules.js";

const duplicate = new TableRowDuplicate();
const submitter = new FormSubmitter();




document.querySelector(".btn-form").addEventListener("click", (e) => {
  e.preventDefault();
  console.log("submit clicked");

  const data = new RowDataCollector().getData();
  
if(data){
submitter.submit({AdmissionForm : data});
}
  
});


