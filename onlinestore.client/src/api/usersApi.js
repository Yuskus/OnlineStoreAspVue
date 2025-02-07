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

    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfUsers): ', error);
        alert('Проблемы с сервером!');
    }
}

export const logIn = async (logForm) => {
    try {

    } catch (error) {
        console.error('Ошибка при аутентификации (logIn): ', error);
        alert('Проблемы с сервером!');
    }
}

export const registerCustomer = async (customer) => {
    try {

    } catch (error) {
        console.error('Ошибка при регистрации заказчика (registerCustomer): ', error);
        alert('Проблемы с сервером!');
    }
}

export const registerManager = async (manager) => {
    try {

    } catch (error) {
        console.error('Ошибка при регистрации менеджера (registerManager): ', error);
        alert('Проблемы с сервером!');
    }
}

export const updateUser = async (userId, newUser) => {
    try {

    } catch (error) {
        console.error('Ошибка при обновлении данных (updateUser): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteUser = async (userId) => {
    try {

    } catch (error) {
        console.error('Ошибка при удалении данных (deleteUser): ', error);
        alert('Проблемы с сервером!');
    }
}