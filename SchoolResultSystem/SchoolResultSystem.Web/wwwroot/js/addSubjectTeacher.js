document.addEventListener('DOMContentLoaded', function () {
    const subjects = window.subjectsData || [];
    const tableBody = document.querySelector('#subjectTeacherTable tbody');
    const form = document.getElementById('subjectTeacherForm');

    // Helper: get currently selected subjects
    function getSelectedSubjects() {
        return Array.from(document.querySelectorAll('.subject-select'))
                    .map(sel => sel.value)
                    .filter(val => val);
    }

    // Update subject name when selection changes
    tableBody.addEventListener('change', function (e) {
        if (!e.target.classList.contains('subject-select')) return;

        const selectedValue = e.target.value;
        const row = e.target.closest('tr');

        // Check for duplicates
        const selectedSubjects = getSelectedSubjects();
        const duplicates = selectedSubjects.filter(v => v === selectedValue);
        if (duplicates.length > 1) {
            alert("This subject is already selected in another row.");
            e.target.value = '';
            row.querySelector('.subject-name').textContent = '';
            return;
        }

        const subject = subjects.find(s => s.SubjectCode === selectedValue);
        row.querySelector('.subject-name').textContent = subject ? subject.SubjectName : '';
    });

    // Add new row
    tableBody.addEventListener('click', function (e) {
        if (!e.target.classList.contains('add-row')) return;

        const currentRow = e.target.closest('tr');
        const newRow = currentRow.cloneNode(true);

        // Clear selects and subject name
        newRow.querySelector('.subject-select').value = '';
        newRow.querySelector('.teacher-select').value = '';
        newRow.querySelector('.subject-name').textContent = '';

        // Change + button to − for removing
        const btn = newRow.querySelector('button');
        btn.textContent = '−';
        btn.classList.remove('add-row');
        btn.classList.add('remove-row');

        tableBody.appendChild(newRow);
    });

    // Remove row
    tableBody.addEventListener('click', function (e) {
        if (!e.target.classList.contains('remove-row')) return;

        const row = e.target.closest('tr');
        tableBody.removeChild(row);
    });

    // Final duplicate check on form submit
    form.addEventListener('submit', function (e) {
        const selectedSubjects = getSelectedSubjects();
        const uniqueSubjects = Array.from(new Set(selectedSubjects));
        if (selectedSubjects.length !== uniqueSubjects.length) {
            e.preventDefault();
            alert("You have duplicate subjects in the table. Remove duplicates before submitting.");
        }
    });
});
