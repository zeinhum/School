import { FetchJson, SendData } from "./fetchJson.js";

export async function MoveStudents(id) {
  const layout = document.querySelector(".partial-container");
  const selected = document.querySelectorAll(".select:checked");

  const nsnArray = Array.from(selected).map((s) => s.value);
  console.log(nsnArray);

  if (nsnArray.length === 0) {
    layout.innerHTML = "<p>No students selected</p>";
    return;
  }

  // Fetch class object
  const classData = await FetchJson(
    `/Microservices/Microservicess/ClassObject`
  );

  if (classData) {
    layout.innerHTML = `
            <div>
                <h4>Selected: ${nsnArray.length}</h4>
                <h5>Select the class to move the students to.</h5>
                <select>
                    <option value="">Select class</option>
                    ${classData.classes
                      .map(
                        (element) =>
                          `<option value="${element.classId}">${element.className}</option>`
                      )
                      .join("")}
                </select>
                <button class="move">Move</button>
            </div>
        `;

    document.querySelector(".move").addEventListener("click", async (e) => {
      e.preventDefault();
      const select = layout.querySelector("select");
      const classId = select.value;

      if (!classId) {
        alert("Please select a class.");
        return;
      }

      const payload = {
        FromClass: parseInt(id),
        Toclass: parseInt(classId),
        MovingStudents: nsnArray, 
      };

      const result = await SendData(
        `/Microservices/Microservicess/MoveStudents`,
        payload
      );

      if (result) {
        layout.innerHTML = `<p>${result.message}</p>`;
      } else {
        layout.innerHTML = "<p>Some error occurred, please try again</p>";
      }
    });
  } else {
    layout.innerHTML = "<p>Could not fetch class data</p>";
  }
}
