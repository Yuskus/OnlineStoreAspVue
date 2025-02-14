import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfUsers = async (pageNumber, pageSize) => {
    try {
        validatePages(pageNumber, pageSize);

        const response = await axios.get(`${API_URL}/api/users/getpage?pageNumber=${pageNumber}&pageSize=${pageSize}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfUsers): ', error);
        throw error;
    }
}

export const logIn = async (logForm) => {
    try {
        validateUserRequest(logForm);

        const response = await axios.post(`${API_URL}/api/users/login`, logForm);

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при аутентификации (logIn): ', error);
        throw error;
    }
}

export const registerCustomer = async (customer) => {
    try {
        validateCustomerRegisterRequest(customer);

        const response = await axios.post(`${API_URL}/api/users/registercustomer`, customer);

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при регистрации заказчика (registerCustomer): ', error);
        throw error;
    }
}

export const registerManager = async (manager) => {
    try {
        validateUserRequest(manager);

        const response = await axios.post(`${API_URL}/api/users/registermanager`, manager, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при регистрации менеджера (registerManager): ', error);
        throw error;
    }
}

export const updateUser = async (username, newUser) => {
    try {
        validateUsername(username);
        validateUserInfo(newUser);

        const response = await axios.put(`${API_URL}/api/users/update/${username}`, newUser, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateUser): ', error);
        throw error;
    }
}

export const deleteUser = async (username) => {
    try {
        validateUsername(username);

        const response = await axios.delete(`${API_URL}/api/users/delete/${username}`, {
            headers: {
                'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
        });

        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteUser): ', error);
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

const validateUserInfo = (user) => {
    validateUsername(user.username);
    validateRole(user.role);
}

const validateUserRequest = (user) => {
    validateUsername(user.username);
    validatePassword(user.password);
}

const validateCustomerRegisterRequest = (user) => {
    validateUsername(user.username);
    validatePassword(user.password);
    validateCustomerName(user.customerInfo.name);
    validateCustomerCode(user.customerInfo.code);
}

const validatePages = (pageNumber, pageSize) => {
    const isValid = Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 36;
    if (!isValid) throw new Error('Ошибка валидации pageNumber, pageSize.');
}

const validateUsername = (username) => {
    const isValid = username && username.trim().length > 6;
    if (!isValid) throw new Error('Ошибка валидации username.');
}

const validatePassword = (password) => {
    const isValid = password && password.length > 6;
    if (!isValid) throw new Error('Ошибка валидации password.');
}

const validateRole = (role) => {
    const isValid = role === 0 || role === 1;
    if (!isValid) throw new Error('Ошибка валидации role.');
}

const validateCustomerCode = (code) => { 
    let isValid = code && /^[0-9]{4}-[0-9]{4}$/i.test(code)
    if (isValid) {
        let rightPart = parseInt(code.split('-')[1]);
        isValid &&= !isNaN(rightPart) && rightPart > 1900 && rightPart < new Date().getFullYear();
    }
    if (!isValid) throw new Error('Ошибка валидации code.');
}

const validateCustomerName = (name) => {
    const isValid = name && name.trim().length > 0;
    if (!isValid) throw new Error('Ошибка валидации name.');
}