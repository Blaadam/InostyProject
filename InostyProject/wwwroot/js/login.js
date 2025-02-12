async function main() {
    var response = await fetch('@Url.Action("GetCurrentUser", "User")')
    if (!response.ok) {
        return;
    }

    const user = await response.json();

    // Logic to handle logout
    if (user.accountName != null) {
        document.querySelector('.logout-button').addEventListener('click', async function () {
            var response = await fetch('@Url.Action("Logout", "User")')

            if (!response.ok) {
                return;
            }

            window.location.href = '/';
        });
    }
    // Logic to handle login
    else {
        var logoutBtn = document.querySelector('.logout-button')
        logoutBtn.textContent = "Login"

        logoutBtn.addEventListener('click', function () {
            window.location.href = '/User/Login'
        });
    }
}

main();
