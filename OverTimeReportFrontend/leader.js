/**
 * POPUP LOADING AND ERROR ELEMS
 */
const loadingOverlay = document.getElementById("loading-overlay");
const floatingError = document.getElementById("floatingErr");
const errMsg = document.getElementById("floating-msg");

// FORM ELEMENTS
let allOvertimeData = [];
let solvers = new Set();
let statuses = new Set();


// EVENT LISTENERS
// Modify the event listeners
document.addEventListener('DOMContentLoaded', () => {
    setGreetingInfo();
    validateToken();
});

// Add event listeners for filter buttons
document.getElementById('button-filter').addEventListener('click', applyFilters);
document.getElementById('button-defilter').addEventListener('click', removeFilters);


window.addEventListener('resize', () => {
    let greetingInfo = document.getElementById('greetingLeadInfo');
    adjustTextSize(greetingInfo);
});


// FUNCTIONS
function setGreetingInfo() {
    try {
        const leadData = JSON.parse(sessionStorage.getItem('leadData'));
        let greetingInfo = document.getElementById('greetingLeadInfo');

        greetingInfo.innerHTML = '';

        const name = leadData.name.toLowerCase().split(' ').map(capitalize).join(' ');

        let nameStrong = document.createElement("span");
        nameStrong.textContent = `Hola, ${name}`;
        nameStrong.style.display = "block";

        greetingInfo.appendChild(nameStrong);

        let welcomeSpan = document.createElement("strong");
        welcomeSpan.textContent = "A continuacion te presentamos las solicitudes por revisar";
        welcomeSpan.style.display = "block";

        greetingInfo.appendChild(welcomeSpan);

        // Llamar a la función para ajustar el texto
        adjustTextSize(greetingInfo);

    } catch (error) {
        console.error('Ocurrió un error:', error);
    }
}

function adjustTextSize(element) {
    const maxWidth = element.offsetWidth;
    const maxHeight = element.offsetHeight;
    let fontSize = parseInt(window.getComputedStyle(element).fontSize);

    while (element.scrollWidth > maxWidth || element.scrollHeight > maxHeight) {
        fontSize--;
        element.style.fontSize = `${fontSize}px`;
        if (fontSize < 8) break; // Establece un límite mínimo de tamaño de fuente
    }
}

function capitalize(name) {
    return name.charAt(0).toUpperCase() + name.slice(1);
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
            await fetchOvertimeData(token);
            populateFilters();
            displayOvertimeData(allOvertimeData);
        }
    } catch (error) {
        console.error("Something went wrong:", error);
        showError(error.message);
    }
}


// Add these new functions
async function fetchOvertimeData(token) {
    try {
        const response = await fetch('https://localhost:7045/api/overtime/all', {
            headers: {
                'Authorization': `Bearer ${token}`
            }
        });
        const data = await response.json();
        allOvertimeData = data.data;
        allOvertimeData.forEach(item => {
            solvers.add(item.solId);
            statuses.add(item.status);
        });
    } catch (error) {
        console.error('Error fetching overtime data:', error);
        showError('Error al obtener datos de horas extras');
    }
}

function populateFilters() {
    const solverOptions = document.getElementById('solverOptions');
    const statusOptions = document.getElementById('statusOptions');

    solverOptions.innerHTML = ''; 

    solvers.forEach(solver => {
        let option = document.createElement('p');
        option.textContent = solver;
        solverOptions.appendChild(option);
    });

    setOptionsClickEvent(document.getElementById('solverSelect'));

    statuses.forEach(status => {
        let option = document.createElement('p');
        option.textContent = status;
        statusOptions.appendChild(option);
    });

    setOptionsClickEvent(document.getElementById('statusSelect'));
}

function selectOption(selectId, value) {
    const select = document.getElementById(selectId);
    const selectedSpan = select.querySelector('.fancy-selected span');
    selectedSpan.textContent = value;
    toggleDropdown(selectId);
}

function toggleDropdown(elem) {
    elem.getElementsByClassName("dropdown-content")[0].classList.toggle("show");
    elem.getElementsByClassName("imgDownArrow")[0].classList.toggle("rotate");
    elem.getElementsByClassName("select")[0].classList.toggle("fancy-selected-dropped");
}

function setOptionsClickEvent(elem) {
    let opts = elem.getElementsByTagName("p");
    for (let opt of opts) {
        opt.addEventListener("click", function () {
            let selected = elem.getElementsByClassName("fancy-selected")[0].getElementsByTagName("span")[0];
            selected.textContent = opt.textContent;
            toggleDropdown(elem);
            var event = new Event("change");
            selected.dispatchEvent(event);
        });
    }
}

function applyFilters() {
    const selectedDate = document.getElementById('fechaSolicitud').value;
    const selectedSolver = document.getElementById('selectedSolver').textContent;
    const selectedStatus = document.getElementById('selectedStatus').textContent;

    let filteredData = allOvertimeData;

    if (selectedDate) {
        const formattedDate = formatDate(selectedDate);
        filteredData = filteredData.filter(item => item.notificationDate === formattedDate);
    }

    if (selectedSolver !== 'Seleccionar SOLVER') {
        filteredData = filteredData.filter(item => item.solId === selectedSolver);
    }

    if (selectedStatus !== 'Seleccionar estado') {
        filteredData = filteredData.filter(item => item.status === selectedStatus);
    }

    displayOvertimeData(filteredData);
}

function formatDate(dateString) {
    const [year, month, day] = dateString.split('-');
    const dayInt = parseInt(day, 10);
    return `${dayInt}/${month}/${year}`;
}

function removeFilters() {
    document.getElementById('selectedSolver').textContent = 'Seleccionar SOLVER';
    document.getElementById('selectedStatus').textContent = 'Seleccionar estado';
    document.getElementById('fechaSolicitud').value = ''; 
    displayOvertimeData(allOvertimeData);
}

function displayOvertimeData(data) {
    const tableBody = document.querySelector('table tbody');
    tableBody.innerHTML = '';

    data.forEach(item => {
        const row = document.createElement('tr');

        // Create action buttons
        const editButton = createActionButton('editing.png', 'Editar', () => handleEdit(item));
        const viewButton = createActionButton('eye.png', 'Ver', () => handleView(item));

        // Show the correct button based on the status
        if (item.status === 'Pendiente') {
            editButton.style.display = 'inline-block';  // Show Edit button
            viewButton.style.display = 'none';  // Hide View button
        } else if (['Aprobado', 'Rechazado'].includes(item.status)) {
            editButton.style.display = 'none';  // Hide Edit button
            viewButton.style.display = 'inline-block';  // Show View button and enable it
            viewButton.disabled = false;
        } else {
            // Hide both buttons if the status doesn't match the specified ones
            editButton.style.display = 'none';
            viewButton.style.display = 'none';
        }

        row.innerHTML = `
            <td>${item.notificationDate}</td>
            <td>${item.solId}</td>
            <td>${item.solName}</td>
            <td>${item.detail}</td>
            <td>${item.reportDate}</td>
            <td>${item.status}</td>
            <td class="action-buttons"></td>
        `;

        const actionCell = row.querySelector('.action-buttons');
        actionCell.appendChild(editButton);
        actionCell.appendChild(viewButton);
        tableBody.appendChild(row);
    });
}

function createActionButton(iconSrc, tooltip, onClick) {
    const button = document.createElement('button');
    const icon = document.createElement('img');
    icon.src = `src/${iconSrc}`;
    icon.alt = tooltip;
    icon.style.width = '20px';  // Ajusta este valor según sea necesario
    icon.style.height = '20px'; // Ajusta este valor según sea necesario
    button.appendChild(icon);
    button.className = 'button-with-descr';
    button.title = tooltip;
    button.onclick = onClick;
    return button;
}

function handleEdit(item) {
    console.log("Item a editar:", item); // Agregar esta línea
    // Asegúrate de usar el campo correcto para el ID
    const id = item.id || item.solId || item.requestId; // Usa el que esté disponible
    if (id) {
        window.location.href = `overtimeRequest.html?id=${id}`;
    } else {
        console.error("No se pudo encontrar un ID válido para la solicitud");
        showError("Error al abrir la solicitud: ID no encontrado");
    }
}

async function handleView(item) {
    const modal = document.getElementById("summaryModal");
    const span = document.getElementsByClassName("close")[0];
    const summaryContent = document.getElementById("summaryContent");

    try {
        loadingOverlay.classList.remove('invisible');
        const leadData = JSON.parse(sessionStorage.getItem("leadData"));
        const token = leadData ? leadData.token : null;

        if (!token) {
            throw new Error('No se encontró token de autenticación');
        }

        const response = await fetch(`https://localhost:7045/api/overtime/${item.id}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json",
            },
        });

        const data = await response.json();

        if (data.success) {
            summaryContent.innerHTML = `
                <p><strong>SOLID:</strong> ${data.data.solId}</p>
                <p><strong>Nombre:</strong> ${data.data.solName}</p>
                <p><strong>Sede:</strong> ${data.data.reportHq}</p>
                <p><strong>Fecha de Novedad:</strong> ${data.data.noveltyDate}</p>
                <p><strong>Hora Inicio:</strong> ${data.data.initialHour}</p>
                <p><strong>Hora Fin:</strong> ${data.data.finalHour}</p>
                <p><strong>Horas Extras:</strong> ${data.data.hour}</p>
                <p><strong>Detalle:</strong> ${data.data.detail}</p>
                <p><strong>Estado:</strong> ${data.data.status}</p>
                <p><strong>Comentarios:</strong> ${data.data.approvalComment || 'N/A'}</p>
            `;
            modal.style.display = "block";
        } else {
            showError("Error al cargar el resumen de la solicitud");
        }
    } catch (error) {
        console.error("Something went wrong:", error);
        showError(error.message);
    } finally {
        loadingOverlay.classList.add('invisible');
    }

    span.onclick = function () {
        modal.style.display = "none";
    }

    window.onclick = function (event) {
        if (event.target == modal) {
            modal.style.display = "none";
        }
    }
}
