import { RowDataCollector, TableRowDuplicate } from "./FormModules.js";
import { FetchJson, FetchJsonPost } from "./fetchJson.js";
import { SelectOption } from "./buttons/selectOption.js";
import { PartialNaV } from "./UI/partialNav.js";

export default class Examrubrick {
  constructor() {
    this.dataCollector = new RowDataCollector();
    //this.duplicateRow = new TableRowDuplicate();
    this.selectOp = new SelectOption();
    this.selectWrapper = document.getElementById("select-wraper");
    this.classWrapper = document.getElementById("class-wraper");
    this.tableBody = document.querySelector(".form-data");
    this.exam = [];

    this.map = {
      selectExam: this.#selectExam.bind(this),
      newExam: this.#newExam.bind(this),
      setNewExam: this.#setNewExam.bind(this),
      selectClass: this.#selectClass.bind(this),
      selectedClass: this.#onClassChange.bind(this),
      selectSchool : this.#selectSchool.bind(this),
      remove: this.remove,
      submit: this.#submitRubric.bind(this),
    };

    this.nav = new PartialNaV(".partial-container", this.map);
  }

  async #selectExam() {
    this.selectWrapper.innerHTML = "";
    this.selectOp.innerElement(this.selectWrapper, "select", {
      "id": "existing-exam",
    });

    try {
      if (this.exam.length === 0) {
        this.exam = await FetchJson("/Microservices/ExamRubrick/GetExamList");
        console.log(this.exam);
      }
      if (!this.exam?.length) {
        this.selectOp.addOption("none", "No exam found");
        return;
      }

      this.selectOp.addOption("none", "— select exam —");
      this.exam.forEach(({ examId, examName, academicYear }) =>
        this.selectOp.addOption(examId, `${examName} – ${academicYear}`),
      );
    } catch (err) {
      console.log(err);
      alert("Failed to load exams");
    }
  }

  async #newExam() {
    this.selectWrapper.innerHTML = `
      <div class="group">
        <label for="AcademicYear">Academic Year</label>
        <input class="year small" type="text" name="AcademicYear"
               placeholder="current year" required />
      </div>
      <div class="group">
        <label for="ExamName">Exam Name</label>
        <input class="examname small" type="text" name="ExamName"
               required placeholder="Exam Name" />
      </div>
      <button id="new-exam" class="btn-success" data-action="setNewExam">Create</button>`;
  }

  async #setNewExam() {
    const year = parseInt(document.querySelector(".year")?.value, 10);
    const exName = document.querySelector(".examname")?.value?.trim();

    if (!exName || isNaN(year)) {
      alert("Please fill in both Academic Year and Exam Name.");
      return;
    }

    try {
      const res = await FetchJsonPost("/Microservices/ExamRubrick/CreateExam", {
        AcademicYear: year,
        ExamName: exName,
      });
      alert(res.message);
      this.selectWrapper.innerHTML = "";
    } catch (err) {
      console.log("Failed to create exam.", err);
      alert("Exam not set");
    }
  }

  async #selectClass() {
    try {
      const { classes } =
        (await FetchJson("/Microservices/Microservicess/AllClasses")) ?? {};
      console.log(classes);
      if (classes.length === 0) {
        this.classWrapper.innerHTML = "<p>No class found.</p>";
        return;
      }

      this.classWrapper.innerHTML = "";
      this.selectOp.innerElement(this.classWrapper, "select", {
        class: "classes",
        id: "classes",
        "data-change": "selectedClass",
      });
      this.selectOp.addOption("none", "— select class —");
      classes.forEach(({ classId, className, classGrade }) =>
        this.selectOp.addOption(classId, `${className} – ${classGrade}`),
      );
    } catch (err) {
      console.log(err);
      alert("Error Occured");
    }
  }

  async #selectSchool() {
    try {
      const subjects = await FetchJson(
        "/Microservices/ExamRubrick/GetAllSubjects",
      );
      if (subjects.length === 0) {
        alert("No active subjects found in the school.");
        return;
      }
      this.classWrapper.innerHTML = "";
      this.#fillSubjects(subjects);
    } catch (err) {
      console.log("Failed to load subjects.", err);
    }
  }

  async #onClassChange(e) {
    const classId = e.target.value;
    if (classId === "none") return;

    try {
      const subs = await FetchJsonPost("/Microservices/ExamRubrick/ClassSubs", {
        Id: classId,
      });
      this.#fillSubjects(subs);
      if (!subs?.length) {
        alert("No Active Subject Found.");
        return;
      }
    } catch (err) {
      console.log("Failed to load class subjects.", err);
    }
  }

  async #submitRubric() {
    const examSelect = document.getElementById("existing-exam");
    if (!examSelect || examSelect.value === "none") {
      alert("Select an exam first.");
      return;
    }

    const payload = {
      ...this.#collectRubricData(),
      ExamId: parseInt(examSelect.value, 10),
    };

    if (!payload.Subs) {
      return;
    }
    console.log(payload)
    
    try {
      const res = await FetchJsonPost(
        "/Microservices/ExamRubrick/CreateRubrick",
        payload,
      );
      alert(res.message);
      this.tableBody.innerHTML = "";
    } catch (err) {
      alert("failed to save rubric");
      console.log("Failed to save rubric.", err);
    }
  }

  // helpers
  #fillSubjects(subs) {
    this.tableBody.innerHTML = "";
    this.tableBody.innerHTML = subs
      .map(
        ({ sCode, sName }) => `
      <tr>
        <td>
          <select class="Scode" name="SCode" required>
            <option value="${sCode}" selected>${sName} (${sCode})</option>
          </select>
        </td>
        <td><input type="number" name="FullMark"   required /></td>
        <td><input type="number" name="CreditHour" required /></td>
        <td><button class="btn-remove" data-action="remove"> - </button></td>
      </tr>`,
      )
      .join("");
  }

  remove(e) {
    const row = e.target.closest("tr");
    row.remove();
  }

  #collectRubricData() {
    return { Subs: this.dataCollector.getData() };
  }

  destroy() {
    this.nav.destroy(); // cleans up PartialNaV listeners

    this.selectOp = null;
    this.selectWrapper = null;
    this.classWrapper = null;
    this.tableBody = null;
    this.dataCollector = null;
    this.exam = [];
    this.map = null;
    this.nav = null;

    console.log("Examrubrick destroyed");
  }
}
