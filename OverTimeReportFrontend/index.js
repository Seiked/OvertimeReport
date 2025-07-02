/**
 * POPUP LOADING AND ERROR ELEMS
 */
const loadingOverlay = document.getElementById("loading-overlay");
const floatingError = document.getElementById("floatingErr");
const errMsg = document.getElementById("floating-msg");

// FORM ELEMENTS
const FORM_ELEMENTS = {
    noveltyDate: document.getElementById('fecha-reporte'),
    headquarter: document.getElementById('sede-actividad'),
    initialHour: document.getElementById('hora-inicial'),
    initialMinute: document.getElementById('minutos-inicial'),
    finalHour: document.getElementById('hora-fin'),
    finalMinute: document.getElementById('minutos-fin'),
    detailId: document.getElementById('detalle-horas-extra'),
    otherDetail: document.getElementById('otro-detalle'),
    medioReporte: document.getElementById('medio-reporte'),
    ticketOrEmailInformation: document.getElementById('ticket-asunto'),
    observations: document.getElementById('observations')
};

// EVENT LISTENERS
document.addEventListener('DOMContentLoaded', () => {
    setGreetingInfo();
    populateHoras();
    validateToken();
    setReviewButtonVisibility();
    populateInitialHourFilter(); 
    populateInitialMinuteFilter();
    populateFinalHourFilter();    
    populateFinalMinuteFilter();
});

FORM_ELEMENTS.detailId.addEventListener('change', mostrarCampoOtro);
document.getElementById('guardar-button').addEventListener('click', submitForm);

// FUNCTIONS
function setGreetingInfo() {
    try {
        const leadData = JSON.parse(sessionStorage.getItem('leadData'));
        let greetingInfo = document.getElementById('greetingLeadInfo');

        greetingInfo.innerHTML = '';

        const name = leadData.name.toLowerCase().split(' ').map(capitalize).join(' ');

        let solIdSpan = document.createElement("span");
        solIdSpan.textContent = `${leadData.solId} - `;
        greetingInfo.appendChild(solIdSpan);

        let nameStrong = document.createElement("span");
        nameStrong.textContent = name;
        greetingInfo.appendChild(nameStrong);

        let positionTitle = leadData.positionTittle.toLowerCase().split(' ').map(capitalize).join(' ');

        let positionSpan = document.createElement("span");
        positionSpan.textContent = positionTitle;
        positionSpan.style.display = "block";
        positionSpan.style.textAlign = "left";
        greetingInfo.appendChild(positionSpan);

    } catch (error) {
        console.error('Ocurrió un error:', error);
    }
}

function setReviewButtonVisibility() {
    try {
        const leadData = JSON.parse(sessionStorage.getItem('leadData'));
        const reviewButton = document.getElementById('reviewButton');
        
        if (reviewButton && leadData && leadData.roles) {
            if (!leadData.roles.includes('Leader')) {
                reviewButton.style.display = 'none';
            }
        }
    } catch (error) {
        console.error('Ocurrió un error al configurar la visibilidad del botón de revisión:', error);
    }
}

function capitalize(name) {
    return name.charAt(0).toUpperCase() + name.slice(1);
}

function toggleVisibility(element, show) {
    element.classList.toggle('hidden', !show);
}

function mostrarCampoOtro() {
    const detalleHorasExtra = FORM_ELEMENTS.detailId.value;
    const campoOtro = document.getElementById('campo-otro');
    const actividadProgramada = document.getElementById('actividad-programada');
    const ticketAsuntoCont = document.getElementById('ticket-asunto-cont');

    campoOtro.classList.toggle('hidden', detalleHorasExtra !== '6');
    const showProgramada = ['5', '4'].includes(detalleHorasExtra);
    actividadProgramada.classList.toggle('hidden', !showProgramada);
    ticketAsuntoCont.classList.toggle('hidden', !showProgramada);
}

function validateForm() {
    const errors = [];
    const today = new Date();
    const noveltyDate = new Date(FORM_ELEMENTS.noveltyDate.value);
    const maxNoveltyDate = new Date(today);
    maxNoveltyDate.setDate(maxNoveltyDate.getDate() - 3);

    const initialHourDate = new Date(`1970-01-01T${FORM_ELEMENTS.initialHour.value}:00`);
    const finalHourDate = new Date(`1970-01-01T${FORM_ELEMENTS.finalHour.value}:00`);

    if (!FORM_ELEMENTS.noveltyDate.value) {
        errors.push('Debe seleccionar una fecha de reporte.');
    } else if (noveltyDate > today) {
        errors.push('La fecha de reporte no puede ser en el futuro.');
    } else if (noveltyDate < maxNoveltyDate) {
        errors.push('Recuerda que el plazo máximo de reporte es de 2 días.');
    }

    else if (!FORM_ELEMENTS.headquarter.value || FORM_ELEMENTS.headquarter.value === "0") {
        errors.push('Debe seleccionar una sede.');
    }

    else if (!FORM_ELEMENTS.initialHour.value) {
        errors.push('Debe seleccionar una hora de inicio.');
    }

    else if (!FORM_ELEMENTS.finalHour.value) {
        errors.push('Debe seleccionar una hora de fin.');
    } else if (initialHourDate >= finalHourDate) {
        errors.push('La hora fin debe ser mayor a la hora de inicio, por favor valide la información.');
    }

    else if (!FORM_ELEMENTS.detailId.value || FORM_ELEMENTS.detailId.value === "0") {
        errors.push('Debe seleccionar un detalle de horas extra.');
    } else if (FORM_ELEMENTS.detailId.value === '6' && !FORM_ELEMENTS.otherDetail.value) {
        errors.push('Debe especificar el detalle de horas extra.');
    }

    else if (['5', '4'].includes(FORM_ELEMENTS.detailId.value)) {
        if (!FORM_ELEMENTS.medioReporte.value || FORM_ELEMENTS.medioReporte.value === "0" || !FORM_ELEMENTS.ticketOrEmailInformation.value || !FORM_ELEMENTS.observations.value) {
            errors.push('Recuerda completar todos los campos.');
        }
    }

    else if (!FORM_ELEMENTS.observations.value) {
        errors.push('Debe proporcionar observaciones.');
    }



    return errors;
}

function prepareFormDataForBackend() {
    const medioReporte = FORM_ELEMENTS.medioReporte.value;
    const horaInicial = `${FORM_ELEMENTS.initialHour.value}:${FORM_ELEMENTS.initialMinute.value}`;
    const horaFinal = `${FORM_ELEMENTS.finalHour.value}:${FORM_ELEMENTS.finalMinute.value}`;

    return {
        noveltyDate: new Date(FORM_ELEMENTS.noveltyDate.value).toISOString(),
        headquarter: FORM_ELEMENTS.headquarter.value,
        initialHour: horaInicial,
        finalHour: horaFinal,
        detailId: parseInt(FORM_ELEMENTS.detailId.value),
        otherDetail: FORM_ELEMENTS.otherDetail.value,
        byTicket: medioReporte === 'ticket',
        byEmail: medioReporte === 'email',
        ticketOrEmailInformation: FORM_ELEMENTS.ticketOrEmailInformation.value,
        observations: FORM_ELEMENTS.observations.value
    };
}

function resetForm() {
    FORM_ELEMENTS.noveltyDate.value = '';
    FORM_ELEMENTS.headquarter.value = '0';
    FORM_ELEMENTS.initialHour.value = '00:00';
    FORM_ELEMENTS.finalHour.value = '00:00';
    FORM_ELEMENTS.detailId.value = '0';
    FORM_ELEMENTS.otherDetail.value = '';
    FORM_ELEMENTS.medioReporte.value = '0';
    FORM_ELEMENTS.ticketOrEmailInformation.value = '';
    FORM_ELEMENTS.observations.value = '';

    // Hide optional fields
    document.getElementById('campo-otro').classList.add('hidden');
    document.getElementById('actividad-programada').classList.add('hidden');
    document.getElementById('ticket-asunto-cont').classList.add('hidden');
}

async function submitForm() {
    const errors = validateForm();
    if (errors.length > 0) {
        showError(errors.join('\n'));
        return;
    }

    const formData = prepareFormDataForBackend();

    try {
        loadingOverlay.classList.remove('invisible');

        const leadData = JSON.parse(sessionStorage.getItem("leadData"));
        const token = leadData ? leadData.token : null;

        if (!token) {
            throw new Error('No authentication token found');
        }

        const response = await fetch('https://localhost:7045/api/overtime/report', {
            method: 'POST',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(formData)
        });

        const data = await response.json();

        if (data.success) {
            showSuccess('Formulario guardado exitosamente.');
            resetForm(); // Add this line to reset the form after successful save
        } else {
            showError(data.errors.join('\n'));
        }
    } catch (error) {
        console.error('An error occurred:', error);
        showError('Ocurrió un error al enviar el formulario.');
    } finally {
        loadingOverlay.classList.add('invisible');
    }
}


function showError(errorMsg) {
    floatingError.classList.replace("invisible", "visible");
    floatingError.classList.replace("floating-success", "floating-err");
    errMsg.textContent = errorMsg;
}

function showSuccess(message) {
    floatingError.classList.replace("invisible", "visible");
    floatingError.classList.replace("floating-err", "floating-success");
    errMsg.textContent = message;
}

function hideFloating() {
    floatingError.classList.add('invisible');
}

function populateHoras() {
    const horaInicialSelect = FORM_ELEMENTS.initialHour;
    const minutosInicialSelect = FORM_ELEMENTS.initialMinute;
    const horaFinSelect = FORM_ELEMENTS.finalHour;
    const minutosFinSelect = FORM_ELEMENTS.finalMinute;

    for (let i = 0; i < 24; i++) {
        let hour = i.toString().padStart(2, '0');
        let optionHora = document.createElement('option');
        optionHora.value = hour;
        optionHora.textContent = hour;

        horaInicialSelect.appendChild(optionHora.cloneNode(true));
        horaFinSelect.appendChild(optionHora.cloneNode(true));
    }

    for (let j = 0; j < 60; j += 5) {
        let minute = j.toString().padStart(2, '0');
        let optionMinuto = document.createElement('option');
        optionMinuto.value = minute;
        optionMinuto.textContent = minute;

        minutosInicialSelect.appendChild(optionMinuto.cloneNode(true));
        minutosFinSelect.appendChild(optionMinuto.cloneNode(true));
    }
}


async function populateDetalleHorasExtras(token) {
    try {
        const response = await fetch('https://localhost:7045/api/overtime/details', {
            method: 'GET',
            headers: {
                'Authorization': `Bearer ${token}`,
                'Content-Type': 'application/json',
            }
        });

        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }

        const data = await response.json();

        if (data.success && data.data) {
            data.data.forEach(item => {
                let option = document.createElement('option');
                option.value = item.value;
                option.textContent = item.text;
                FORM_ELEMENTS.detailId.appendChild(option);
            });
        } else {
            console.error('Error in response data:', data.errors);
        }
    } catch (error) {
        console.error('An error occurred:', error);
    }
}

async function validateToken() {
    const leadData = JSON.parse(sessionStorage.getItem("leadData"));
    let token = leadData ? leadData.token : null;

    if (!token) {
        window.location.href = "login.html";
        return;
    }

    try {
        const response = await fetch(`https://localhost:7045/api/auth/validatetoken/${token}`, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
            },
            body: JSON.stringify(leadData),
        });

        const data = await response.json();

        if (!data.data) {
            window.location.href = "login.html";
            sessionStorage.setItem("leadData", JSON.stringify(data.data));
        } else {
            await populateDetalleHorasExtras(token);
        }
    } catch (error) {
        console.error("Something went wrong:", error);
        showError(error.message);
    }
}

/**
 *
 * @param {*} elem searchbar HTML element
 * Swithches visibility for dropdown options and searchbar
 */

function toggleDropdown(elem) {
  elem.getElementsByClassName("dropdown-content")[0].classList.toggle("show");
  elem.getElementsByClassName("imgDownArrow")[0].classList.toggle("rotate");
  elem
    .getElementsByClassName("select")[0]
    .classList.toggle("fancy-selected-dropped");
}
 
function addOptions(options, elem, values = []) {
  for (let i = 0; i < options.length; i++) {
    const opt = options[i];
    let option = document.createElement("p");
    option.textContent = opt;
    if (values.length > 0) {
      option.id = values[i];
    }
    elem.getElementsByClassName("options-cont")[0].appendChild(option);
  }
}
 
function setOptionsClickEvent(elem) {
  let opts = elem.getElementsByTagName("p");
  for (let opt of opts) {
    opt.addEventListener("click", function () {
      let selected = elem.getElementsByClassName("selected")[0];
      let value = elem.getElementsByClassName("value")[0];
      selected.value = opt.textContent;
      value.value = opt.id;
      toggleDropdown(elem);
      var event = new Event("change");
      selected.dispatchEvent(event);
    });
  }
}
 
function filterFunction(elem) {
  const input = elem.getElementsByClassName("opt-search")[0];
  const filter = input.value.toUpperCase();
  const dropdownContent = elem.getElementsByClassName("options-cont")[0];
  const links = dropdownContent.getElementsByTagName("p");
 
  for (let i = 0; i < links.length; i++) {
    const linkText = links[i].textContent || links[i].innerText;
    if (linkText.toUpperCase().indexOf(filter) > -1) {
      links[i].style.display = "";
    } else {
      links[i].style.display = "none";
    }
  }
}

/**
 *
 * @param {*} elem searchbar HTML element
 * Swithches visibility for dropdown options and searchbar
 */

function toggleDropdown(elem) {
  elem.getElementsByClassName("dropdown-content")[0].classList.toggle("show");
  elem.getElementsByClassName("imgDownArrow")[0].classList.toggle("rotate");
  elem
    .getElementsByClassName("select")[0]
    .classList.toggle("fancy-selected-dropped");
}
 
function addOptions(options, elem, values = []) {
  for (let i = 0; i < options.length; i++) {
    const opt = options[i];
    let option = document.createElement("p");
    option.textContent = opt;
    if (values.length > 0) {
      option.id = values[i];
    }
    elem.getElementsByClassName("options-cont")[0].appendChild(option);
  }
}
 
function setOptionsClickEvent(elem) {
    let opts = elem.getElementsByTagName("p");
    for (let opt of opts) {
        opt.addEventListener("click", function () {
            let selected = elem.getElementsByClassName("selected")[0];
            let value = elem.getElementsByClassName("value")[0];
            selected.value = opt.textContent;
            value.value = opt.id;
            toggleDropdown(elem);
            var event = new Event("change");
            selected.dispatchEvent(event);

            // Update FORM_ELEMENTS.initialHour if this is the initialHourFilter
            if (elem.id === 'initialHourFilter') {
                FORM_ELEMENTS.initialHour.value = opt.textContent;
            }
            // Update FORM_ELEMENTS.initialMinute if this is the initialMinuteFilter
            else if (elem.id === 'initialMinuteFilter') {
                FORM_ELEMENTS.initialMinute.value = opt.textContent;
            }
            if (elem.id === 'finalHourFilter') {
                FORM_ELEMENTS.finalHour.value = opt.textContent;
            }
            // Update FORM_ELEMENTS.initialMinute if this is the initialMinuteFilter
            else if (elem.id === 'finalMinuteFilter') {
                FORM_ELEMENTS.finalMinute.value = opt.textContent;
            }
        });
    }
}
 
function filterFunction(elem) {
  const input = elem.getElementsByClassName("opt-search")[0];
  const filter = input.value.toUpperCase();
  const dropdownContent = elem.getElementsByClassName("options-cont")[0];
  const links = dropdownContent.getElementsByTagName("p");
 
  for (let i = 0; i < links.length; i++) {
    const linkText = links[i].textContent || links[i].innerText;
    if (linkText.toUpperCase().indexOf(filter) > -1) {
      links[i].style.display = "";
    } else {
      links[i].style.display = "none";
    }
  }
}

function populateInitialHourFilter() {
    const initialHourFilter = document.getElementById("initialHourFilter");
    const optionsCont = initialHourFilter.querySelector('.options-cont');
    optionsCont.innerHTML = ''; // Clear existing options

    for (let i = 0; i < 24; i++) {
        let hour = i.toString().padStart(2, '0');
        let option = document.createElement('p');
        option.textContent = hour;
        option.id = hour;
        optionsCont.appendChild(option);
    }

    setOptionsClickEvent(initialHourFilter);
}

function populateInitialMinuteFilter() {
    const initialMinuteFilter = document.getElementById("initialMinuteFilter");
    const optionsCont = initialMinuteFilter.querySelector('.options-cont');
    optionsCont.innerHTML = ''; // Clear existing options

    for (let i = 0; i < 60; i += 5) {
        let minute = i.toString().padStart(2, '0');
        let option = document.createElement('p');
        option.textContent = minute;
        option.id = minute;
        optionsCont.appendChild(option);
    }

    setOptionsClickEvent(initialMinuteFilter);
}

function populateFinalHourFilter() {
    const finalHourFilter = document.getElementById("finalHourFilter");
    const optionsCont = finalHourFilter.querySelector('.options-cont');
    optionsCont.innerHTML = ''; // Clear existing options

    for (let i = 0; i < 24; i++) {
        let hour = i.toString().padStart(2, '0');
        let option = document.createElement('p');
        option.textContent = hour;
        option.id = hour;
        optionsCont.appendChild(option);
    }

    setOptionsClickEvent(finalHourFilter);
}

function populateFinalMinuteFilter() {
    const finalMinuteFilter = document.getElementById("finalMinuteFilter");
    const optionsCont = finalMinuteFilter.querySelector('.options-cont');
    optionsCont.innerHTML = ''; // Clear existing options

    for (let i = 0; i < 60; i += 5) {
        let minute = i.toString().padStart(2, '0');
        let option = document.createElement('p');
        option.textContent = minute;
        option.id = minute;
        optionsCont.appendChild(option);
    }

    setOptionsClickEvent(finalMinuteFilter);
}