export class Attendance {
  constructor() {
    this.classHeader = document.querySelector("[data-classId]");

    this.tableBody = document.querySelector("#attendence-table tbody");
    this.StudsRows = this.tableBody.querySelectorAll("tr");

    this.PresentAbsentSwitch();
  }

  PresentAbsentSwitch() {
    if (!this.tableBody) {
      console.error("Attendance table body not found.");
      return;
    }

    // Sync present checkbox and absent reason
    this.StudsRows.forEach((row) => {
      const presentCheckbox = row.querySelector(".present-check input");
      const absentSelect = row.querySelector(".absent-reason-selection");

      // When user marks present → clear absent reason
      presentCheckbox.addEventListener("change", () => {
        if (presentCheckbox.checked) {
          absentSelect.value = "none";
        }
      });

      // When user selects an absent reason → uncheck present
      absentSelect.addEventListener("change", () => {
        if (absentSelect.value !== "none") {
          presentCheckbox.checked = false;
        }
      });
    });
  }

  GetAttendenceData() {
    
    const classStuds = [];

    this.classId = this.classHeader
      ? this.classHeader.getAttribute("data-classId")
      : null;
    this.classId = parseInt(this.classId);

    this.teacherId = document.querySelector("[data-userid]").dataset.userid;

    this.StudsRows.forEach((row) => {
      const nsnCell = row.querySelector(".nsn");
      const presentCheckbox = row.querySelector(".present-check input");
      const absentSelect = row.querySelector(".absent-reason-selection");

      const nsn = nsnCell.textContent.trim();
      const absentReason = absentSelect.value;
      const present = absentReason === "none";

      classStuds.push({
        nsn: nsn,
        present: present,
        absent: present ? "N/A" : absentReason,
      });
    });

    const attenceData = {
      ClassId: this.classId,
      teacherId: this.teacherId,
      studs: classStuds,
    };

    return JSON.stringify(attenceData);
  }

  async PostApiCall(apiUrl, data) {
    const response = await fetch(apiUrl, {
      method: "POST",
      headers: { "Content-Type": "application/json" },
      body: data,
    });
    return response;
  }

  async SubmitAddendence(url) {
    const SubmitButton = document.querySelector(".submit-attendence");
    SubmitButton.addEventListener("click", async (e) => {
      e.preventDefault();
      const AttendencePayload = this.GetAttendenceData();
      const response = await this.PostApiCall(url);
      if(response.ok){
        confirm("Attendence Done!");
      }else{
        alert("Attendence Not Submited !!");
        
      }
    });
  }
}
