import { TableRowDuplicate, FormSubmitter, RowDataCollector } from "./FormModules.js";

const duplicate = new TableRowDuplicate();
const submitter = new FormSubmitter();




document.querySelector(".btn-form").addEventListener("click", (e) => {
  e.preventDefault();

  const data = new RowDataCollector().getData();
  console.log(JSON.stringify(data))
  
if(data){
submitter.submit(data);
}
  
});


