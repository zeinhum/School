export function Attendance() {

    const classHeader = document.querySelector('[data-classId]');
    let classId = classHeader ? classHeader.getAttribute('data-classId') : null;
    classId= parseInt(classId);

    let teacherId = document.querySelector("[data-userid]").dataset.userid;



    const tableBody = document.querySelector('#attendence-table tbody');
    if (!tableBody) {
        console.error("Attendance table body not found.");
        return;
    }

    const rows = tableBody.querySelectorAll("tr");

    // Sync present checkbox and absent reason
    rows.forEach(row => {
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

    document.querySelector(".submit-attendence").addEventListener("click", () => {

        const classStuds = [];

        rows.forEach(row => {
            const nsnCell = row.querySelector(".nsn");
            const presentCheckbox = row.querySelector(".present-check input");
            const absentSelect = row.querySelector(".absent-reason-selection");

            const nsn = nsnCell.textContent.trim();
            const absentReason = absentSelect.value;
            const present = absentReason === "none";

            classStuds.push({
                nsn: nsn,
                present: present,
                absent: present ? "N/A" : absentReason
            });
        });

        const attenceData = {
            ClassId: classId,
            teacherId: teacherId,
            studs: classStuds
        };

        console.log("Attendance Data:");
        console.log(JSON.stringify(attenceData));
    });
}
