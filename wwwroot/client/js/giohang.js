document.addEventListener('DOMContentLoaded', function() {
    const removeButtons = document.querySelectorAll('.remove-btn');
    const incrementButtons = document.querySelectorAll('.increment');
    const decrementButtons = document.querySelectorAll('.decrement');

    removeButtons.forEach(button => {
        button.addEventListener('click', function() {
            const row = this.closest('tr');
            row.remove();
            updateCartTotal();
        });
    });

    incrementButtons.forEach(button => {
        button.addEventListener('click', function() {
            const quantityElement = this.previousElementSibling;
            let quantity = parseInt(quantityElement.textContent);
            quantity++;
            quantityElement.textContent = quantity;
            updateRowTotal(this);
            updateCartTotal();
        });
    });

    decrementButtons.forEach(button => {
        button.addEventListener('click', function() {
            const quantityElement = this.nextElementSibling;
            let quantity = parseInt(quantityElement.textContent);
            if (quantity > 1) {
                quantity--;
                quantityElement.textContent = quantity;
                updateRowTotal(this);
                updateCartTotal();
            }
        });
    });

    function updateRowTotal(element) {
        const row = element.closest('tr');
        const price = parseFloat(row.cells[2].textContent.replace('$', ''));
        const quantity = parseInt(row.querySelector('.quantity').textContent);
        const total = price * quantity;
        row.querySelector('.item-total').textContent = total.toFixed(2) + ' $';
    }

    function updateCartTotal() {
        let subtotal = 0;
        const rows = document.querySelectorAll('tbody tr');
        rows.forEach(row => {
            const total = parseFloat(row.querySelector('.item-total').textContent.replace('$', ''));
            subtotal += total;
        });

        document.querySelector('.subtotal').textContent = `Subtotal: $${subtotal.toFixed(2)}`;
        const total = subtotal + 3; // Assuming a flat shipping rate of $3
        document.querySelector('.total').textContent = `Total: $${total.toFixed(2)}`;
    }
});
