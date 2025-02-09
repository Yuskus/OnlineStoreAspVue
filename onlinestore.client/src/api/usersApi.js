import axios from 'axios';

const API_URL = 'http://localhost:5000';

export const getPageOfUsers = async (pageNumber, pageSize) => {
    try {
        if (!checkPages(pageNumber, pageSize)) throw new Error("Ошибка валидации страниц: pageNumber, pageSize (Users).");

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
        if (!checkUsername(username)) throw new Error("Ошибка при валидации никнейма (Users).");
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
        if (!checkUsername(user.username)) throw new Error("Ошибка при валидации никнейма (Users).");

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
    let validationErrors = [];

    if (!checkUsername(user.username)) validationErrors.push("Ошибка валидации username (user).");
    if (!checkRole(user.role)) validationErrors.push("Ошибка валидации role (user).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации User");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const validateUserRequest = (user) => {
    let validationErrors = [];

    if (!checkUsername(user.username)) validationErrors.push("Ошибка валидации username (user).");
    if (!checkPassword(user.password)) validationErrors.push("Ошибка валидации password (user).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации User");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const validateCustomerRegisterRequest = (user) => {
    let validationErrors = [];

    if (!checkUsername(user.username)) validationErrors.push("Ошибка валидации username (user).");
    if (!checkPassword(user.password)) validationErrors.push("Ошибка валидации password (user).");
    if (!checkCustomerName(user.customerInfo.name)) validationErrors.push("Ошибка валидации customerInfo.name (user).");
    if (!checkCustomerCode(user.customerInfo.code)) validationErrors.push("Ошибка валидации customerInfo.code (user).");

    if (validationErrors.length > 0) {
        let error = new Error("Ошибки валидации User");
        error.validationErrors = validationErrors;
        throw error;
    }
}

const checkPages = (pageNumber, pageSize) => {
    return Number.isInteger(pageNumber) && Number.isInteger(pageSize) && pageNumber > 0 && pageSize > 0 && pageSize <= 36;
}

const checkUsername = (username) => {
    return username && username.trim() > 6;
}

const checkPassword = (password) => {
    return password && password.length > 6;
}

const checkRole = (role) => {
    return role === 0 || role === 1;
}

const checkCustomerCode = (code) => { 
    if (code && /^[0-9]{4}-[0-9]{4}$/i.test(code)) {
        let rightPart = parseInt(code.split('-')[1]);
        return !isNaN(rightPart) && rightPart > 1900 && rightPart < new Date().getFullYear();
    }
    return false;
}

const checkCustomerName = (name) => {
    return name && name.trim() > 0;
}