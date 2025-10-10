import { TableRowDuplicate, FormSubmitter, RowDataCollector } from "./FormModules.js";

const duplicate = new TableRowDuplicate();
const submitter = new FormSubmitter();




document.querySelector(".btn-form").addEventListener("click", (e) => {
  e.preventDefault();
  console.log("submit clicked");

  const data = getExamData();
  console.log("Final data to send:", JSON.stringify(data, null, 2));
if(data){
submitter.submit(data);
}
  
});

function getExamData() {
  const data = {};

  // ✅ Get year and exam name (non-table fields)
  const nonTabFields = document.querySelectorAll(".year, .examname");
  for (const field of nonTabFields) {
    const name = field.name?.trim();
    const value = field.value?.trim();

    if (!value) {
      alert("Some fields are empty. Please fill all required fields.");
      return null; 
    }

    if (name==="AcademicYear"){ data[name] = parseInt(value,10)}else{
      data[name]=value;
    };

  }
  data["SubjectMarks"]= new RowDataCollector().getData();
  return data;
}
