

// Confirm before copying the database
    document.getElementById("copy-db")?.addEventListener("click", function (e) {
        const ok = confirm("Are you sure you want to copy the database?");
        if (!ok) {
            e.preventDefault();
            alert("Database copy canceled.");
        }else{
            window.location.href = "/Principal/SetupPrincipal/CopyDb";
           
        }
    });

    // Confirm before replacing the database
    document.getElementById("replace-db")?.addEventListener("click", function (e) {
        const file = document.getElementById("file").value;

        if (!file) {
            e.preventDefault();
            alert("Please choose a .db file before replacing the database.");
            return;
        }

        const ok = confirm("⚠️ This will replace the existing database.\nDo you want to continue?");
        if (!ok) {
            e.preventDefault();
            alert("Database replacement canceled.");
            return
        }
        if (ok) {
            document.getElementById("replace-form").submit();
        }
    });

     
        