import { FetchJsonPost } from "./fetchJson.js";
import {PartialNaV } from "./UI/partialNav.js"


export default class MoveTeachers {
  constructor(){
    this.maper = {
      changepassword:this.changePassword,
      cancelchange: this.cancelChange,
      confirmchange:this.confirmChange,
      deactivateteacher: this.deactivateTeacher
    }
    this.nav = new PartialNaV (".partial-container",this.maper);

  }
  destroy(){
    this.nav.destroy();
  }
  // Business Logic 

  async changePassword(id) {
    const payload = { Id: id };

    const teacher = await FetchJsonPost(
      "/Microservices/Microservicess/TeacherId",
      payload
    );

    this.root.innerHTML = `
      <div class="card">
        <h3>Change Password</h3>
        <p>
          Name: <strong>${teacher.teacherName} (${teacher.teacherId})</strong>
          <br/>
          Username: <strong>${teacher.userName}</strong>
        </p>
        <input id="newPw" type="password" placeholder="New password" class="pwd"/>
        <div class="actions">
          <button data-action="confirmchange">Change</button>
          <button data-action="cancelchange">Cancel</button>
        </div>
      </div>
    `;

  }

  async confirmChange(){
    const input = this.root.querySelector("#newPw");
        const NewPw = input?.value?.trim();

        if (!NewPw) {
          alert("Password cannot be empty.");
          return;
        }

        payload.NewPw = NewPw;

        const res = await FetchJsonPost(
          "/Microservices/Microservicess/ChangePassword",
          payload
        );

        this.root.innerHTML = res.message;
  }
  async cancelChange(){
    this.root.innerHTML = "Password unchanged.";
  }
  

  async deactivateTeacher(id) {
    const payload = { Id: id };

    const res = await FetchJsonPost(
      "/Microservices/Microservicess/DeactivateTeacher",
      payload
    );

    this.root.innerHTML = res.message;
  }

  async removeCST(button) {
    if (!button) return;

    const header = document.querySelector(".header");
    const classId = parseInt(header?.dataset?.classid, 10);
    if (!classId) return;

    const row = button.closest("tr");
    if (!row) return;

    const cells = row.querySelectorAll("td");
    if (cells.length < 3) return;

    const rowData = {
      SCode: cells[0].textContent.trim(),
      TeacherId: cells[2].textContent.trim(),
      ClassId: classId,
    };

    const res = await FetchJsonPost(
      "/Microservices/Microservicess/RemoveSubjectTeacher",
      rowData
    );

    alert(res.message);
  }
}

