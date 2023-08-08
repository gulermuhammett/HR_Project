function onlyAlphabets(event) {
    var key = event.keyCode || event.which;
    var keyChar = String.fromCharCode(key);
    var regex = /^[A-Za-z]+$/; // Sadece harf karakterlerini kabul eden düzenli ifade

    if (
        !regex.test(keyChar) &&
        key !== 8 &&
        key !== 46 &&
        key !== 37 &&
        key !== 39 &&
        key !== 38 &&
        key !== 40
    ) {
        event.preventDefault();
    }
}

function onlyPositiveNumbers(event) {
    var key = event.keyCode || event.which;
    var keyChar = String.fromCharCode(key);
    var regex = /^[0-9]+$/; // Sadece "0" ve pozitif sayıları kabul eden düzenli ifade

    if (
        !regex.test(keyChar) &&
        key !== 8 &&
        key !== 46 &&
        key !== 37 &&
        key !== 39 &&
        key !== 38 &&
        key !== 40
    ) {
        event.preventDefault();
    }
}


function phoneNumber() {
    var phoneInput = document.getElementById('phone');
    var myForm = document.forms.myForm;
    var result = document.getElementById('result');  // only for debugging purposes

    phoneInput.addEventListener('input', function (e) {
        var x = e.target.value.replace(/\D/g, '').match(/(\d{0,3})(\d{0,3})(\d{0,4})/);
        e.target.value = !x[2] ? x[1] : '(' + x[1] + ') ' + x[2] + (x[3] ? '-' + x[3] : '');
    });

    myForm.addEventListener('submit', function (e) {
        phoneInput.value = phoneInput.value.replace(/\D/g, '');
        result.innerText = phoneInput.value;  // only for debugging purposes

        e.preventDefault(); // You wouldn't prevent it
    });
}