textAreaAdjust();
function textAreaAdjust() {
  let textareas = document.getElementsByTagName("textarea");
  for (let i = 0; i < textareas.length; i++) {
    const element = textareas[i];
    const parent = element.parentElement.parentElement.parentElement;
    parentHeight = parent.offsetHeight;
    element.style.height = "1px";
    element.style.height = element.scrollHeight - 15 + "px";
    parent.style.height = "auto";
  }
}

function encodeInput(input) {
  const encoded = document.createElement("div");
  encoded.innerText = input;
  return encoded.innerHTML;
}

function hideFloating() {
  floatingError.classList.replace("visible", "invisible");
}

function validateToken() {
  const leadData = JSON.parse(sessionStorage.getItem("leadData"));
  let token;
  if (leadData === null || leadData === "") {
    window.location.href = "login.html";
  } else {
    token = leadData.token;
  }

  if (token !== null || token !== "") {
    fetch("https://localhost:7045/api/auth/validatetoken/" + token, {
      method: "POST",
      headers: {
        "Content-Type": "application/json",
      },
      body: JSON.stringify(leadData),
    })
      .then((response) => response.json())
      .then((data) => {
        loadingOverlay.classList.replace("visible", "invisible");
        if (!data.data) {
          window.open("login.html", "_self");
          sessionStorage.setItem("leadData", JSON.stringify(data.data));
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
}



function adjustWolfie() {
  let welcomeCard = document.getElementsByClassName("welcome-card");
  welcomeCard[0].children[0].style.height = 0;
  let parentHeight = welcomeCard[0].clientHeight;
  welcomeCard[0].children[0].style.height = parentHeight + "px";
}



function showError(errmess) {
  loadingOverlay.classList.replace("visible", "invisible");
  floatingError.classList.replace("invisible", "visible");
  floatingError.classList.replace("floating-success", "floating-err");
  errMsg.textContent = errmess;
}

function showSuccess(successMsg) {
  loadingOverlay.classList.replace("visible", "invisible");
  floatingError.classList.replace("invisible", "visible");
  floatingError.classList.replace("floating-err", "floating-success");
  errMsg.textContent = successMsg;
}

// Define a custom error constructor
class CustomError {
  constructor(message, name) {
    this.name = name;
    this.message = message || "Algo salío mal";
    this.stack = new Error().stack;
  }
}
CustomError.prototype = Object.create(Error.prototype);
