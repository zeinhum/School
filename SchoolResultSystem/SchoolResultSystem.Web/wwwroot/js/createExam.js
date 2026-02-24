import { TableRowDuplicate, RowDataCollector } from "./FormModules.js";
import { FetchJson, FetchJsonPost } from "./fetchJson.js";
import { ClickOn, SelectOption } from "./buttons/selectOption.js";

const duplicate = new TableRowDuplicate();
const click = new ClickOn();
const selectOp = new SelectOption();

// populate exam form
click.element(".select-existing", async () => {
  if (!document.getElementById("existing-exam")) {
    let wraperEl =document.getElementById("select-wraper");
    wraperEl.innerHTML = ""
    selectOp.createEl(
      wraperEl,
      "select",
      "existing-exam",
    );
  }
  //const res = await FetchJson("/Microservices/ExamRubrick/ExamList");
  const res = [{ExamId:2, ExamName: "Test Exam", AcademicYear:2026}]
  const prEl = document.querySelector("#existing-exam");
  res.forEach((element) => {
    selectOp.addOption(
      element.ExamId,
      `${element.ExamName}-${element.AcademicYear}`,
      prEl,
    );
  });
});
// new exam
click.element(".newexam", () => {
  document.getElementById("select-wraper").innerHTML =
    `<div class="group">
                        <label for="AcademicYear"> Academic Year</label>
                        <input class="year small" type="text" name="AcademicYear" placeholder="current year" required />
                    </div>
                    <div class="group">
                        <lebel formaction="ExamName">Exam Name</lebel>
                        <input class="examname small" type="text" name="ExamName" required placeholder="Exam Name" />
                    </div>
                    <button id="new-exam" class="actions btn-success">Create</button>`;
});

// Create exam

click.element("#new-exam", async () => {
  let year = document.querySelector(".year");
  year = parseInt(year.value, 10);
  let exName = document.querySelector(".examname");
  exName = exName.value;
  const payload = { AcademicYear: year, ExamName: exName };

  const res = await FetchJsonPost(
    "/Microservices/ExamRubrick/CreateExam",
    payload,
  );
  alert(res.message);
  document.querySelector(".new-exam").innerHTML = "";
});

// set rubrick for all subject
click.element("#set-all", () => {
  const ids = ["Thmark", "Prmark", "ThCrh", "PrCrh"];
  const marks = [];
  ids.forEach((el) => {
    marks.push(document.getElementById(el).value);
  });

  const select = document.getElementById("Scode");
  const tableRow = document.querySelector(".form-data");
  

  const subjects = Array.from(select.options).map((option) => ({
    name: option.text,
    scode: option.value,
  }));
  subjects.shift();
  console.log(subjects)
  tableRow.innerHTML="";

  subjects.forEach((value) => {
    
const tr = document.createElement("tr");
    tr.innerHTML = `
<td>
    <select class="Scode" name="SCode" required>
        <option value="${value.scode}" selected>${value.name}</option>
    </select>
</td>
<td><input type="number" name="ThMark" value="${marks[0]}" required size="2" /></td>
<td><input type="number" name="PrMark" value="${marks[1]}" required size="2" /></td>
<td><input type="number" name="ThCrh" value="${marks[2]}" required size="2" /></td>
<td><input type="number" name="PrCrh" value="${marks[3]}" required size="2" /></td>
<td class="btn-group">
<button type="button" class="btn-remove">-</button>
</td>
`;

    tableRow.appendChild(tr);
  });
  removeRow();
});

// create rubrick
click.element("#create-rubrik", async (e) => {
  const data = getSubjectRubrik();
  let ExamId = document.querySelector("#existing-exam");
  ExamId = parseInt(ExamId.value);
  data.ExamId = ExamId;
  console.log(data);
  /*
  const res = await FetchJsonPost(
    "/Microservices/ExamRubrick/CreateRubrick",
    data,
  ); 
  alert(res.message); */
});

function getSubjectRubrik() {
  const data = {};
  const dataCollect = new RowDataCollector();
  const rowData = dataCollect.getData();
  data["SubjectMarks"] = rowData;
  return data;
}

// remove button 
function removeRow() {
  document.addEventListener("click", (e) => {
    const btn = e.target.closest(".btn-remove");
    if (!btn) return; // click wasn't on remove button

    const row = btn.closest("tr");
    if (row) row.remove();
  });
}
