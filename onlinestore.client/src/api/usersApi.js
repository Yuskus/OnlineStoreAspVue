import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Users/getall?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Users/login [body]
// http://localhost:5000/api/Users/registercustomer [body]
// http://localhost:5000/api/Users/registermanager [body]
// http://localhost:5000/api/users/update/{this.user.customerId} [body]
// http://localhost:5000/api/users/delete/${this.user.customerId}

export const getPageOfUsers = async (pageNumber, pageSize) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfUsers): ', error);
        alert('Проблемы с сервером!');
    }
}

export const logIn = async (logForm) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при аутентификации (logIn): ', error);
        alert('Проблемы с сервером!');
    }
}

export const registerCustomer = async (customer) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при регистрации заказчика (registerCustomer): ', error);
        alert('Проблемы с сервером!');
    }
}

export const registerManager = async (manager) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при регистрации менеджера (registerManager): ', error);
        alert('Проблемы с сервером!');
    }
}

export const updateUser = async (userId, newUser) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateUser): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteUser = async (userId) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteUser): ', error);
        alert('Проблемы с сервером!');
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