import {
  TableRowDuplicate,
  FormSubmitter,
  RowDataCollector,
} from "./FormModules.js";
import { FetchJson, FetchJsonPost } from "./fetchJson.js";
import { SelectOption } from "./buttons/selectOption.js";

const duplicate = new TableRowDuplicate();
const submitter = new FormSubmitter();
/*
//const selOpt = new SelectOption();

let clicked = false;
const selector = document.getElementById("select-class");
selector.addEventListener("click", async (e) => {
  if (!clicked) {
    const clsObj = await FetchJson("/Microservices/Microservicess/ClassObject");
    clsObj.classes.forEach((cl) => {
      selOpt.addOption(cl.classId, cl.className, selector);
    });
    clicked = true;
  }
});
*/
// submit form
document.querySelector(".btn-form").addEventListener("click", async (e) => {
  e.preventDefault();

  const data = new RowDataCollector().getData();
  //const subdata={ClassId:parseInt(selector.value, 10)};
  //subdata.Subs = data
  console.log(JSON.stringify(subdata));

  if (data) {
    const res = await FetchJsonPost("/Principal/SetupPrincipal/SaveSubject");
    alert(res);
  }
});
