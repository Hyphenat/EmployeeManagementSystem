// Fade in animation on page load
$(document).ready(function () {
    $('body').addClass('fade-in');
});

// Confirm delete with SweetAlert
function confirmDelete(url, itemName) {
    Swal.fire({
        title: 'Are you sure?',
        text: `Do you want to delete this ${itemName}?`,
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#e74a3b',
        cancelButtonColor: '#858796',
        confirmButtonText: 'Yes, delete it!',
        cancelButtonText: 'Cancel'
    }).then((result) => {
        if (result.isConfirmed) {
            window.location.href = url;
        }
    });
}

// Form validation
function validateForm(formId) {
    const form = document.getElementById(formId);
    if (!form.checkValidity()) {
        form.classList.add('was-validated');
        return false;
    }
    return true;
}

// Show loading spinner
function showLoading() {
    Swal.fire({
        title: 'Processing...',
        html: 'Please wait',
        allowOutsideClick: false,
        didOpen: () => {
            Swal.showLoading();
        }
    });
}

// Hide loading spinner
function hideLoading() {
    Swal.close();
}

// Format currency
function formatCurrency(amount) {
    return '₹' + amount.toLocaleString('en-IN', {
        minimumFractionDigits: 0,
        maximumFractionDigits: 0
    });
}

// Format date
function formatDate(dateString) {
    const options = { year: 'numeric', month: 'short', day: 'numeric' };
    return new Date(dateString).toLocaleDateString('en-IN', options);
}

// Print function
function printDiv(divId) {
    const printContents = document.getElementById(divId).innerHTML;
    const originalContents = document.body.innerHTML;
    document.body.innerHTML = printContents;
    window.print();
    document.body.innerHTML = originalContents;
    location.reload();
}

// Export table to CSV
function exportTableToCSV(filename, tableId) {
    const csv = [];
    const rows = document.querySelectorAll(`#${tableId} tr`);

    for (let i = 0; i < rows.length; i++) {
        const row = [], cols = rows[i].querySelectorAll('td, th');
        for (let j = 0; j < cols.length; j++) {
            let data = cols[j].innerText.replace(/(\r\n|\n|\r)/gm, '').replace(/(\s\s)/gm, ' ');
            data = data.replace(/"/g, '""');
            row.push('"' + data + '"');
        }
        csv.push(row.join(','));
    }

    const csvFile = new Blob([csv.join('\n')], { type: 'text/csv' });
    const downloadLink = document.createElement('a');
    downloadLink.download = filename;
    downloadLink.href = window.URL.createObjectURL(csvFile);
    downloadLink.style.display = 'none';
    document.body.appendChild(downloadLink);
    downloadLink.click();
}

// Auto-hide alerts after 5 seconds
setTimeout(function () {
    $('.alert').fadeOut('slow');
}, 5000);

// Tooltip initialization (if using Bootstrap tooltips)
$(function () {
    $('[data-toggle="tooltip"]').tooltip();
});

// Smooth scroll to top
function scrollToTop() {
    window.scrollTo({ top: 0, behavior: 'smooth' });
}

// Add to top button functionality
$(window).scroll(function () {
    if ($(this).scrollTop() > 100) {
        $('#scroll-to-top').fadeIn();
    } else {
        $('#scroll-to-top').fadeOut();
    }
});

// Search functionality for tables
function searchTable(inputId, tableId) {
    const input = document.getElementById(inputId);
    const filter = input.value.toUpperCase();
    const table = document.getElementById(tableId);
    const tr = table.getElementsByTagName('tr');

    for (let i = 1; i < tr.length; i++) {
        let found = false;
        const td = tr[i].getElementsByTagName('td');
        for (let j = 0; j < td.length; j++) {
            if (td[j]) {
                const txtValue = td[j].textContent || td[j].innerText;
                if (txtValue.toUpperCase().indexOf(filter) > -1) {
                    found = true;
                    break;
                }
            }
        }
        tr[i].style.display = found ? '' : 'none';
    }
}

// Validate email
function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

// Validate phone number (Indian format)
function validatePhone(phone) {
    const re = /^[6-9]\d{9}$/;
    return re.test(phone);
}

// Show success message
function showSuccess(message) {
    Swal.fire({
        icon: 'success',
        title: 'Success!',
        text: message,
        confirmButtonColor: '#1cc88a'
    });
}

// Show error message
function showError(message) {
    Swal.fire({
        icon: 'error',
        title: 'Error!',
        text: message,
        confirmButtonColor: '#e74a3b'
    });
}

// Show warning message
function showWarning(message) {
    Swal.fire({
        icon: 'warning',
        title: 'Warning!',
        text: message,
        confirmButtonColor: '#f6c23e'
    });
}

// Show info message
function showInfo(message) {
    Swal.fire({
        icon: 'info',
        title: 'Information',
        text: message,
        confirmButtonColor: '#36b9cc'
    });
}

// Disable button after click to prevent multiple submissions
function disableButton(buttonId) {
    const button = document.getElementById(buttonId);
    button.disabled = true;
    button.innerHTML = '<i class="fas fa-spinner fa-spin me-2"></i>Processing...';
}

// Enable button
function enableButton(buttonId, originalText) {
    const button = document.getElementById(buttonId);
    button.disabled = false;
    button.innerHTML = originalText;
}

// Check if date is in future
function isFutureDate(dateString) {
    const inputDate = new Date(dateString);
    const today = new Date();
    today.setHours(0, 0, 0, 0);
    return inputDate > today;
}

// Calculate age from date of birth
function calculateAge(dob) {
    const birthDate = new Date(dob);
    const today = new Date();
    let age = today.getFullYear() - birthDate.getFullYear();
    const monthDiff = today.getMonth() - birthDate.getMonth();
    if (monthDiff < 0 || (monthDiff === 0 && today.getDate() < birthDate.getDate())) {
        age--;
    }
    return age;
}

// Capitalize first letter
function capitalizeFirstLetter(string) {
    return string.charAt(0).toUpperCase() + string.slice(1);
}

// Format phone number for display
function formatPhoneNumber(phoneNumber) {
    return phoneNumber.replace(/(\d{5})(\d{5})/, '$1-$2');
}

// Copy to clipboard
function copyToClipboard(text) {
    navigator.clipboard.writeText(text).then(function () {
        showSuccess('Copied to clipboard!');
    }, function (err) {
        showError('Could not copy text');
    });
}

// Dark mode toggle (if you want to add dark mode feature)
function toggleDarkMode() {
    document.body.classList.toggle('dark-mode');
    const isDark = document.body.classList.contains('dark-mode');
    localStorage.setItem('darkMode', isDark);
}

// Load dark mode preference
$(document).ready(function () {
    const darkMode = localStorage.getItem('darkMode');
    if (darkMode === 'true') {
        document.body.classList.add('dark-mode');
    }
});

// Number animation on page load
function animateValue(id, start, end, duration) {
    const obj = document.getElementById(id);
    let startTimestamp = null;
    const step = (timestamp) => {
        if (!startTimestamp) startTimestamp = timestamp;
        const progress = Math.min((timestamp - startTimestamp) / duration, 1);
        obj.innerHTML = Math.floor(progress * (end - start) + start);
        if (progress < 1) {
            window.requestAnimationFrame(step);
        }
    };
    window.requestAnimationFrame(step);
}

// Responsive table (convert to cards on mobile)
function makeTableResponsive() {
    if ($(window).width() < 768) {
        $('.table').addClass('table-responsive');
    } else {
        $('.table').removeClass('table-responsive');
    }
}

$(window).resize(makeTableResponsive);
$(document).ready(makeTableResponsive);

// Form auto-save (draft feature)
function autoSaveForm(formId) {
    const form = document.getElementById(formId);
    const formData = new FormData(form);
    const data = {};
    formData.forEach((value, key) => data[key] = value);
    localStorage.setItem(`draft_${formId}`, JSON.stringify(data));
}

// Load form draft
function loadFormDraft(formId) {
    const draft = localStorage.getItem(`draft_${formId}`);
    if (draft) {
        const data = JSON.parse(draft);
        Object.keys(data).forEach(key => {
            const input = document.querySelector(`#${formId} [name="${key}"]`);
            if (input) {
                input.value = data[key];
            }
        });
        showInfo('Draft loaded. You can continue from where you left.');
    }
}

// Clear form draft
function clearFormDraft(formId) {
    localStorage.removeItem(`draft_${formId}`);
}

console.log('Employee Management System - JavaScript Loaded Successfully');
console.log('Version: 1.0.0');
console.log('Developed with ❤️');