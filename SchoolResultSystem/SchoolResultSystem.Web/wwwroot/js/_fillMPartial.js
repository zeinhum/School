import { RowDataCollector, FormSubmitter, TableRowDuplicate } from "./FormModules.js";
const RowData = new RowDataCollector(".row-data");

// get form data from user
function getExamData() { 
  const data = {};

  // ✅ Get year and exam name (non-table fields)
  const nonTabFields = document.querySelectorAll(".SCode");
  for (const field of nonTabFields) {
    const name = field.dataset.name?.trim();
    const value = field.dataset.value?.trim();

    if (!value) {
      alert("Some fields are empty. Please fill all required fields.");
      return null; 
    }

    if (name==="ExamId" || name==="ClassId"){ data[name] = parseInt(value,10)}else{data[name]=value;};
    
  }
  // ✅ Get selected exam
  const examDropdown = document.getElementById("examDropdown");
  const examId = examDropdown.value.trim();

  if (!examId) {
    alert("Please select an exam.");
    return null;
  }

  data["ExamId"] = parseInt(examId, 10);
  data["Marks"]= RowData.getData();
  if(!data["Marks"]){
    
    return null;
  }
  return data;
}

// initializer
export function initializer(){
     
    document.querySelector(".btn-form").addEventListener("click", (e) => {
  e.preventDefault();
 
  const data = getExamData();

if(data){
SubmitForm.submit(data);
}
  
});
const duplicator = new TableRowDuplicate(".row-data");
const SubmitForm = new FormSubmitter();
}