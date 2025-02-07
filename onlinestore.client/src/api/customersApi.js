import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const updateCustomer = async (customerId, newCustomer) => {
    let validationErrors = [];

    if (!validateGuid(newCustomer.id)) validationErrors.push("Ошибка валидации id (Customer).");
    if (!validateName(newCustomer.name)) validationErrors.push("Ошибка валидации name (Customer).");
    if (!validateCode(newCustomer.code)) validationErrors.push("Ошибка валидации code (Customer).");
    if (!validateDiscount(newCustomer.discount)) validationErrors.push("Ошибка валидации id (Customer).");

    if (errors.length > 0) {
        let error = new Error("Ошибки валидации Customer");
        error.validationErrors = validationErrors;
        throw error;
    }

    try {
        const response = await axios.put(`${API_URL}/api/customers/update/${customerId}`, newCustomer, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        if (response.status === 200 && response.data) {
            console.log("Обновлено.");
            return response.data;
        } else {
            console.log("Статус операции: " + response.status + ".");
            return null;
        }
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateCustomer): ', error);
        throw error;
    }
}

const validateGuid = (id) => {
    id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
}

const validateDiscount = (discount) => {
    discount && Number.isInteger(discount) && discount >= 0 && discount < 100;
}

const validateCode = (code) => { 
    if (code && code.length === 9) {
        let rightPart = code.split('-')[1];
        if (rightPart && Number.isInteger(rightPart)) {
            return rightPart > 1900 && rightPart < new Date().getFullYear();
        }
    }
    return false;
}

const validateName = (name) => {
    return name && name.trim();
}