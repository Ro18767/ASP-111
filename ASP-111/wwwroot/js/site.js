document.addEventListener("DOMContentLoaded", () => {
    const authButton = document.getElementById("auth-button");
    if (authButton) {
        authButton.addEventListener('click', authButtonClick);
    }
    else {
        console.error("Element not found: auth-button");
    }
});

function authButtonClick() {
    const authLogin = document.getElementById("auth-login");
    if (!authLogin) throw "Element not found: auth-login";
    const authPassword = document.getElementById("auth-password");
    if (!authPassword) throw "Element not found: auth-password";
    if (authLogin.value.length === 0) {
        alert("Необхідно ввести логін");
        return;
    }
    if (authPassword.value.length === 0) {
        alert("Необхідно ввести пароль");
        return;
    }
    // передаем данные на сервер асинхронно
    window
        .fetch(                // отсылка запроса
            "/User/Auth", {    // URL - адрес запроса
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                login: authLogin.value,
                password: authPassword.value
            })
        }
        )
        .then(r => r.json())   // ответ => извлечение тела как JSON Объекта
        .then(j => {           // результат извлечения тела
            // console.log(j);
            if (j.success == true) {
                location = location;
                return;
            }
            alert("неверные даные для входа");
        });
}