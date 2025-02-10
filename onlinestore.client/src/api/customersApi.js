import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const updateCustomer = async (customerId, newCustomer) => {
    try {
        validateGuid(customerId);
        validateCustomerRequest(newCustomer);

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
    validateName(newCustomer.name);
    validateCode(newCustomer.code);
    validateDiscount(newCustomer.discount);
}

const validateGuid = (id) => {
    const isValid = id && id.length === 36 && /^[0-9a-fA-F]{8}-([0-9a-fA-F]{4}-){3}[0-9a-fA-F]{12}$/i.test(id);
    if (!isValid) throw new Error('Ошибка валидации guid.');
}

const validateDiscount = (discount) => {
    const isValid = discount % 1 === 0 && discount >= 0 && discount < 100;
    if (!isValid) throw new Error('Ошибка валидации discount.');
}

const validateCode = (code) => {
    let isValid = code && /^[0-9]{4}-[0-9]{4}$/i.test(code);
    if (isValid) {
        let rightPart = parseInt(code.split('-')[1]);
        isValid &&= !isNaN(rightPart) && rightPart > 1900 && rightPart < new Date().getFullYear();
    }
    if(!isValid) throw new Error('Ошибка валидации code.');
}

const validateName = (name) => {
    const isValid = name && name.trim().length > 0;
    if (!isValid) throw new Error('Ошибка валидации name.');
}