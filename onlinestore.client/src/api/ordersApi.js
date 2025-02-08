import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Orders/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Orders/getpagebycustomer/{this.myId}?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Orders/getbasket/${this.myId}
// http://localhost:5000/api/orders/placeanorder/${this.basketOrder.id} []
// http://localhost:5000/api/orders/update/${this.localOrder.id} [body]
// http://localhost:5000/api/orders/delete/${this.order.id}

export const getPageOfOrders = async (pageNumber, pageSize) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfOrders): ', error);
        alert('Проблемы с сервером!');
    }
}

export const getPageOfOrdersByCustomer = async (customerId, pageNumber, pageSize) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfOrdersByCustomer): ', error);
        alert('Проблемы с сервером!');
    }
}

export const getBasket = async (customerId) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при получении данных (getBasket): ', error);
        alert('Проблемы с сервером!');
    }
}

export const placeAnOrder = async (orderId) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при оформлении заказа (placeAnOrder): ', error);
        alert('Проблемы с сервером!');
    }
}

export const updateOrder = async (orderId) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при обновлении данных (updateOrder): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteOrder = async (orderId) => {
    try {
        return handleResponse(response);
    } catch (error) {
        console.error('Ошибка при удалении данных (deleteOrder): ', error);
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