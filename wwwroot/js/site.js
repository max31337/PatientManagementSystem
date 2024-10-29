// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

function submitPaymentForm() {
    const formData = new FormData(form);

    const isPaidCheckbox = form.querySelector('input[asp-for="IsPaid"]');
    if (!isPaidCheckbox.checked) {
        formData.append('IsPaid', 'false'); 
    }

    fetch('/Payment/CreatePayment', {
        method: 'POST',
        body: formData,
    })
        .then((response) => response.json())
        .then((data) => {
            messageDiv.innerHTML = ''; 
            if (data.success) {
                messageDiv.innerHTML = '<span class="text-green-500">Payment added successfully!</span>';
                form.reset(); 
            } else {
                messageDiv.innerHTML = '<span class="text-red-500">' + data.errors.join(', ') + '</span>';
            }
        })
        .catch((error) => {
            console.error('Error:', error);
            messageDiv.innerHTML = '<span class="text-red-500">An error occurred while processing your request.</span>';
        });
}
