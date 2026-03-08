import { TableRowDuplicate, RowDataCollector } from "./FormModules.js";
import { FetchJsonPost } from "./fetchJson.js";

const duplicate = new TableRowDuplicate();





document.querySelector("#submit").addEventListener("click", async (e) => {
  e.preventDefault();
  console.log("submit clicked");

  const data = new RowDataCollector().getData();
  console.log({AdmissionForm :data})
  
if(data){
  
const res =await FetchJsonPost("/Principal/PrincipalDashboard/Admission",{AdmissionForm :data});
alert(res.message);
}
  
});


