const loadingOverlay = document.getElementById("loading-overlay");
const floatingError = document.getElementById("floatingErr");
const errMsg = document.getElementById("floating-msg");

// FORM ELEMENTS
const FORM_ELEMENTS = {
    fechaNovedad: document.getElementById('fecha-novedad'),
    horaInicioJornada: document.getElementById('hora-inicio-jornada'),
    horaFinJornada: document.getElementById('hora-fin-jornada'),
    horasExtras: document.getElementById('horas-extras'),
    sede: document.getElementById('sede'),
    detalleHorasExtras: document.getElementById('detalle-horas-extras'),
    otherDetail: document.getElementById('otro-detalle'),
    medioReporte: document.getElementById('medio-reporte'),
    ticketCorreoAsociado: document.getElementById('ticket-correo-asociado'),
    observaciones: document.getElementById('observaciones'),
    approvalComment: document.getElementById('approval-comment'),
    approveBtn: document.getElementById('approve-btn'),
    rejectBtn: document.getElementById('reject-btn'),
    estado: document.getElementById('estado'),
    firstActivity: document.getElementById('first-activity'),
    lastActivity: document.getElementById('last-activity'),
    entradaBiometrico: document.getElementById('entrada-biometrico'),
    salidaBiometrico: document.getElementById('salida-biometrico')
};


function toggleVisibility(element, show) {
    element.classList.toggle('hidden', !show);
}


// Función para cargar los datos de la solicitud
async function loadOvertimeRequest(id) {
    try {
        loadingOverlay.classList.remove('invisible');

        const leadData = JSON.parse(sessionStorage.getItem("leadData"));
        const token = leadData ? leadData.token : null;

        if (!token) {
            throw new Error('No se encontró token de autenticación');
        }

        const response = await fetch(`https://localhost:7045/api/overtime/${id}`, {
            method: "GET",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json",
            },
        });

        const data = await response.json();

        if (data.success) {
            populateForm(data.data);
            toggleApprovalSection(data.data.status);
        } else {
            showError("Error al cargar la solicitud");
        }
    } catch (error) {
        console.error("Something went wrong:", error);
        showError(error.message);
    } finally {
        loadingOverlay.classList.add('invisible');
    }
}

function mostrarCampoOtro() {
    const detalleHorasExtra = FORM_ELEMENTS.detalleHorasExtras.value;
    const campoOtro = document.getElementById('campo-otro');
    const actividadProgramada = document.getElementById('actividad-programada');
    const ticketAsuntoCont = document.getElementById('ticket-asunto-cont');

    campoOtro.classList.toggle('hidden', detalleHorasExtra !== 'Otro');
    const showProgramada = ['Actividad Infraestructura', 'Movimiento Programado'].includes(detalleHorasExtra);
    actividadProgramada.classList.toggle('hidden', !showProgramada);
    ticketAsuntoCont.classList.toggle('hidden', !showProgramada);
}

function toTitleCase(str) {
    return str.toLowerCase().split(' ').map(word => {
      return word.charAt(0).toUpperCase() + word.slice(1);
    }).join(' ');
  }

function populateForm(data) {
    FORM_ELEMENTS.fechaNovedad.value = data.noveltyDate || '';
    FORM_ELEMENTS.horaInicioJornada.value = data.initialHour || '';
    FORM_ELEMENTS.horaFinJornada.value = data.finalHour || '';
    FORM_ELEMENTS.horasExtras.value = data.hour || '';
    FORM_ELEMENTS.sede.value = data.reportHq || '';
    FORM_ELEMENTS.detalleHorasExtras.value = data.detail || '';
    FORM_ELEMENTS.otherDetail.value = data.otherDetail || '';
    FORM_ELEMENTS.ticketCorreoAsociado.value = data.ticketOrEmailInformation || '';
    FORM_ELEMENTS.observaciones.value = data.observations || '';
    FORM_ELEMENTS.estado.value = data.status || '';
    FORM_ELEMENTS.firstActivity.value = data.activeTrackFirstActivity || '';
    FORM_ELEMENTS.lastActivity.value = data.activeTrackLastActivity || '';
    FORM_ELEMENTS.entradaBiometrico.value = ''; 
    FORM_ELEMENTS.salidaBiometrico.value = '';

    // Mostrar/ocultar campos según el detalle
    mostrarCampoOtro();

    // Manejar la visualización de los campos de ticket/correo
    const actividadProgramada = document.getElementById('actividad-programada');
    const ticketAsuntoCont = document.getElementById('ticket-asunto-cont');
    document.getElementById('nombre-solver').textContent = `${data.solId} - ${data.solName}`;
    document.getElementById('sedesol').textContent = `${data.solHq}`;
    document.getElementById('fecha-generacion').textContent = `${data.creationDate}`;
    document.getElementById('solver-request').textContent = `Solicitud horas extras - ${toTitleCase(data.solName)}`;

    if (data.detail === 'Actividad Infraestructura' || data.detail === 'Movimiento Programado') {
        actividadProgramada.classList.remove('hidden');
        ticketAsuntoCont.classList.remove('hidden');
        
        // Actualizar el título del campo de ticket/correo
        const ticketAsuntoTitle = document.getElementById('ticket-asociado-o-asunto-del-correo');
        if (ticketAsuntoTitle) {
            ticketAsuntoTitle.textContent = data.byTicket ? 'Ticket asociado' : 'Asunto del Correo';
        }

        // Establecer el valor del medio de reporte
        if (data.byTicket) {
            FORM_ELEMENTS.medioReporte.value = 'Ticket';
        } else if (data.byEmail) {
            FORM_ELEMENTS.medioReporte.value = 'Correo';
        }
    } else {
        actividadProgramada.classList.add('hidden');
        ticketAsuntoCont.classList.add('hidden');
    }
}

// Asegúrate de que este event listener esté presente
FORM_ELEMENTS.detalleHorasExtras.addEventListener('change', mostrarCampoOtro);

function toggleApprovalSection(status) {
    const approvalSection = document.getElementById('approval-section');
    if (status === 'Pendiente') {
        approvalSection.classList.remove('hidden');
    } else {
        approvalSection.classList.add('hidden');
    }
}

async function approveRequest(id) {
    await updateRequest(id, 'approve');
}

async function rejectRequest(id) {
    await updateRequest(id, 'reject');
}

async function updateRequest(id, action) {
    const comment = FORM_ELEMENTS.approvalComment.value;
    try {
        loadingOverlay.classList.remove('invisible');

        const leadData = JSON.parse(sessionStorage.getItem("leadData"));
        const token = leadData ? leadData.token : null;

        if (!token) {
            throw new Error('No se encontró token de autenticación');
        }

        const response = await fetch(`https://localhost:7045/api/overtime/${action}/${id}`, {
            method: "PUT",
            headers: {
                "Authorization": `Bearer ${token}`,
                "Content-Type": "application/json",
            },
            body: JSON.stringify({ approvalComment: comment })
        });

        const data = await response.json();

        if (data.success) {
            showSuccess(`Solicitud ${action === 'approve' ? 'aprobada' : 'rechazada'} exitosamente`);
            loadOvertimeRequest(id); // Recargar los datos actualizados
        } else {
            showError(`Error al ${action === 'approve' ? 'aprobar' : 'rechazar'} la solicitud`);
        }
    } catch (error) {
        console.error("Something went wrong:", error);
        showError(error.message);
    } finally {
        loadingOverlay.classList.add('invisible');
    }
}

// Event Listeners
document.addEventListener('DOMContentLoaded', () => {
    const urlParams = new URLSearchParams(window.location.search);
    const id = urlParams.get('id');
    if (id) {
        loadOvertimeRequest(id);
    }

    FORM_ELEMENTS.approveBtn.addEventListener('click', () => approveRequest(id));
    FORM_ELEMENTS.rejectBtn.addEventListener('click', () => rejectRequest(id));
});

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