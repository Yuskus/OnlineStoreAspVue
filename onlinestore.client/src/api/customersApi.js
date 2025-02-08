import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const updateCustomer = async (customerId, newCustomer) => {
    try {
        if (!checkGuid(customerId)) throw new Error("Ошибка валидации Guid (Customer).");
        validateCustomerRequest(customerId, newCustomer);

        const response = await axios.put(`${API_URL}/api/customers/update/${customerId}`, newCustomer, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateCustomer): ', error);
        throw error;
    }
}

const handleResponse = (response) => {
    if (response.status === 200 && response.data) {
        return response.data;
    } else {
        console.log("Статус операции: " + response.status + ".");
        return null;
    }
};

const validateCustomerRequest = (newCustomer) => {
    let validationErrors = [];

    if (!checkName(newCustomer.name)) validationErrors.push("Ошибка валидации name (Customer).");
    if (!checkCode(newCustomer.code)) validationErrors.push("Ошибка валидации code (Customer).");
    if (!checkDiscount(newCustomer.discount)) validationErrors.push("Ошибка валидации discount (Customer).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации Customer");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const checkGuid = (id) => {
    return id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
}

const checkDiscount = (discount) => {
    return Number.isInteger(discount) && discount >= 0 && discount < 100;
}

const checkCode = (code) => { 
    if (code && /^[0-9]{4}-[0-9]{4}$/i.test(code)) {
        let rightPart = parseInt(code.split('-')[1]);
        return !isNaN(rightPart) && rightPart > 1900 && rightPart < new Date().getFullYear();
    }
    return false;
}

const checkName = (name) => {
    return name && name.trim() > 0;
}