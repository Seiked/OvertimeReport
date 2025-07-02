function getData(form) {
  var formData = new FormData(form);
  let leadData = Object.fromEntries(formData);
  loadingOverlay.classList.replace("invisible", "visible");
  fetch("https://localhost:7045/api/auth/login", {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: JSON.stringify(leadData),
  })
    .then((response) => response.json())
    .then((data) => {
      loadingOverlay.classList.replace("visible", "invisible");
      if (!data.success) {
        floatingError.classList.replace("invisible", "visible");
        floatingError.classList.replace("floating-success", "floating-err");
        console.log(data.errors);
        errMsg.textContent = data.errors[0];
      } else {
        sessionStorage.setItem("leadData", JSON.stringify(data.data));
        if (data.data.roles[0] == "TeamMember") {
          location.href = "member.html";
        } else {
          location.href = "member.html";
        }
        if (data.data.roles[1] == "Leader")
          location.href="member.html";
      }
    })
    .catch((error) => {
      console.error("Algo salió mal", error);
      loadingOverlay.classList.replace("visible", "invisible");
      floatingError.classList.replace("invisible", "visible");
      floatingError.classList.replace("floating-success", "floating-err");
      errMsg.textContent = error;
    });
}

const loadingOverlay = document.getElementById("loading-overlay");
const floatingError = document.getElementById("floatingErr");
const errMsg = document.getElementById("floating-msg");

document.getElementById("loginForm").addEventListener("submit", function (e) {
  e.preventDefault();
  getData(e.target);
});
