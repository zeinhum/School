import { FetchJson, FetchJsonPost } from "./fetchJson.js";
import { PartialNaV } from "./UI/partialNav.js";
import { TableRowDuplicate, RowDataCollector } from "./FormModules.js";

export default class ClassStudentTeacher {
  constructor() {
    this.maper = {
      assignsubject: this.#addBaseHtml.bind(this), // Use bind to keep 'this' context
      submitcst: this.#submitcst.bind(this),
    };
    this.subjects = [];
    this.teachers = [];
    this.nav = new PartialNaV(".partial-container", this.maper);
    this. dataCollector;
  }

  // 1. Make this ASYNC so we can wait for data
  async #addBaseHtml() {
    const addForm = document.querySelector(".formholder");
    
    // 2. Wait for the data to actually arrive
    await this.#loadSubject();
    await this.#loadTeacher();

    addForm.innerHTML = `
    <div class="card form-card">
        <h2>Select Subject Teacher</h2>
            <table class="form-card">
                <thead>
                    <tr>
                        <th>Subject</th>
                        <th>Teacher</th>
                        <th>Row</th>
                    </tr>
                </thead>
                <tbody class="form-data">
                    <tr>
                        <td>
                            <select id="subjects" class="Scode" name="SCode" required>
                                <option value="" disabled selected>-Select Subject</option>
                                ${this.subjects.map(({ sCode, sName }) => 
                                    `<option value="${sCode}">${sName} (${sCode})</option>`
                                ).join('')}
                            </select>
                        </td>
                        <td>
                            <select id="teachers" class="UserId" name="UserId" required>
                                <option value="" disabled selected>-Select Teacher</option>
                                ${this.teachers.map(({ userId, teacher }) => 
                                    `<option value="${userId}">${teacher} (${userId})</option>`
                                ).join('')}
                            </select>
                        </td>
                    </tr>
                </tbody>
            </table>
            <button class="btn-success" data-action="submitcst">Assign</button>
    </div>`;
                                
    this.rowDuplicator = new TableRowDuplicate(".form-data");
  }

  
  async #loadSubject() {
    if (this.subjects.length === 0) {
      this.subjects = await FetchJson("/Microservices/Microservicess/AllSubjects");
    }
  }

  async #loadTeacher() {
    if (this.teachers.length === 0) {
      this.teachers = await FetchJson("/Microservices/Microservicess/AllTeachers");
    }
  }

  async #submitcst() {
    if(!this.dataCollector){
        this.dataCollector = new RowDataCollector(".form-data");
    }
    const data = this.dataCollector.getData();
    //console.log(data);
    //const payload ={};
    let classId = document.querySelector(".header");
    classId = parseInt(classId.dataset.classid);
    data.forEach((item)=>{
        item.ClassId=classId
    })

    const res = await FetchJsonPost("/Principal/PrincipalDashboard/AddSubjectTeacher", data);
    alert(res.message);
      
  }

  // destroy method
  destroy() {
    // 1. Safe navigation destroy
    this.nav?.destroy();

    // 2. Safe table duplicator destroy (Fixes your TypeError)
    if (this.rowDuplicator) {
        this.rowDuplicator.destroy();
        this.rowDuplicator = null; 
    }

    // 3. Cleanup other references
    this.dataCollector = null;
    this.subjects = [];
    this.teachers = [];
}
}