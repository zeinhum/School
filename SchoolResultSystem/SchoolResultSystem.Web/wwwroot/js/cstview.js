document.querySelector(".add-cst").addEventListener("click", function () {
  LoadFormData();
});

function popForm(subjects = [], teachers = []) {
  addBaseHtml();

  SubmitForm();
  // Build subject options dynamically
  let subjectOptions = subjects.forEach((sub) => {
    const select = document.querySelector("#SCode");
    // value is the ID/code, text is the name
    const option = document.createElement("option");
    option.value = sub.sCode; // the ID/code sent to server
    option.textContent = `${sub.sName} - (${sub.sCode})`; // display name + id
    select.appendChild(option);
  });

  // Build teacher options dynamically (if you want a select instead of free text)
  let teacherOptions = teachers.forEach((teacher) => {
    const select = document.querySelector("#TeacherId");
    const option = document.createElement("option");
    option.value = teacher.teacherId; // the ID sent to server
    option.textContent = `${teacher.teacherName} - (${teacher.teacherId})`; // display name + id
    select.appendChild(option);
  });
}

function addBaseHtml() {
  const addForm = document.querySelector(".formholder");
  addForm.innerHTML = `
    <div class="card form-card">
        <h2>Add Subject Teacher</h2>
        <form method="post" action="/Principal/PrincipalDashboard/AddSubjectTeacher">
            <input type="hidden" name="ClassId" value="${window.currentClassId}" />

            <div class="form-group">
                <label for="SCode">Subject Code</label>
                <select id="SCode" name="SCode" required>
                <option value="" disabled  selected>Select Subject Code</option>
                </select>
                <label for="TeacherId">Teacher</label>
                <select id="TeacherId" name="TeacherId" required>
                <option value="" disabled selected>Select Teacher</option>
    
                </select>
            </div>

            <button type="submit" class="btn btn-success submit">Add Subject Teacher</button>
        </form>
    </div>`;
}

function LoadFormData() {
  fetch("/Principal/PrincipalDashboard/GetTeachersAndClasses")
    .then((response) => response.json())
    .then((data) => {
      const teachers = data.teachers;
      const subjects = data.subjects; // or data.subjects depending on API
      //window.currentClassId = someClassId; // store for hidden input

      console.log("Fetched teachers:", teachers);
      console.log("Fetched subjects:", subjects);
      popForm(subjects, teachers);
    });
}

function SubmitForm() {
  const submitButton = document.querySelector(".submit");
  submitButton.addEventListener("click", function (event) {
    event.preventDefault(); // prevent default form submission

    const form = submitButton.closest("form");
    const formData = new FormData(form);

    // Convert FormData to JSON
    const data = {};
    formData.forEach((value, key) => {
      data[key] = value;
    });
    console.log("Submitting data:", data);
    fetch(form.action, {
      method: form.method,
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(data),
    })
      .then(async (response) => {
        const result = await response.json(); // parse JSON
        if (response.ok) {
          alert(result.message); // success message from backend
          // optionally, update table dynamically instead of reloading
          form.reset(); // clear the form
        } else {
          alert(result.message || "Error assigning Subject-Teacher.");
        }
      })
      .catch((error) => {
        console.error(error);
        alert("Network or server error.");
      });
  });
}
