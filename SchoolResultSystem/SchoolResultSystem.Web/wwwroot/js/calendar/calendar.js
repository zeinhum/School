class CalendarManager {
    constructor() {
        this.weekendContainer = document.querySelector('.weekend-section');
        this.leaveTableBody = document.querySelector('.leave-table tbody');
    }

    // -----------------------------
    // WEEKEND METHODS
    // -----------------------------

    addWeekendRow() {
        const row = document.createElement('div');
        row.className = 'weekend-row';
        row.innerHTML = `
            <input type="date" />
            <button type="button" class="add-btn weekend-add">+</button>
            <button type="button" class="remove-btn weekend-remove">-</button>
        `;

        this.weekendContainer.appendChild(row);
        this.attachWeekendEvents(row);
    }

    attachWeekendEvents(row) {
        const addBtn = row.querySelector('.weekend-add');
        const removeBtn = row.querySelector('.weekend-remove');

        addBtn.addEventListener('click', () => this.addWeekendRow());
        removeBtn.addEventListener('click', () => row.remove());
    }

    // -----------------------------
    // LEAVE METHODS
    // -----------------------------

    addLeaveRow() {
        const tr = document.createElement('tr');
        tr.innerHTML = `
            <td><input type="date" /></td>
            <td><input type="text" placeholder="Reason" /></td>
            <td>
                <button type="button" class="add-btn leave-add">+</button>
                <button type="button" class="remove-btn leave-remove">-</button>
            </td>
        `;

        this.leaveTableBody.appendChild(tr);
        this.attachLeaveEvents(tr);
    }

    attachLeaveEvents(tr) {
        const addBtn = tr.querySelector('.leave-add');
        const removeBtn = tr.querySelector('.leave-remove');

        addBtn.addEventListener('click', () => this.addLeaveRow());
        removeBtn.addEventListener('click', () => tr.remove());
    }

    // -----------------------------
    // EXTRACT DATA
    // -----------------------------

    extractData() {
        const weekends = [];
        const leaves = [];

        // Weekend extraction
        document.querySelectorAll('.weekend-row').forEach(row => {
            const dateInput = row.querySelector('input[type="date"]');
            if (dateInput && dateInput.value) {
                weekends.push(dateInput.value);
            }
        });

        // Leave extraction
        document.querySelectorAll('.leave-table tbody tr').forEach(row => {
            const dateInput = row.querySelector('input[type="date"]');
            const descInput = row.querySelector('input[type="text"]');

            if (dateInput && dateInput.value) {
                leaves.push({
                    date: dateInput.value,
                    description: descInput.value || ""
                });
            }
        });

        return {
            weekends,
            leaves
        };
    }
}


// Initialize system after DOM loads
document.addEventListener("DOMContentLoaded", () => {
    const cm = new CalendarManager();

    // Attach initial buttons
    document.querySelectorAll('.weekend-row').forEach(row =>
        cm.attachWeekendEvents(row)
    );
    document.querySelectorAll('.leave-table tbody tr').forEach(tr =>
        cm.attachLeaveEvents(tr)
    );

    // Extract button
    const submitBtn = document.getElementById("set-calendar");
    if (submitBtn) {
        submitBtn.addEventListener("click", () => {
            const data = cm.extractData();
            console.log(data);
            alert("Extracted! Check console.");
        });
    }
});
