import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/OrderElements/getbyid/${this.basketOrder.id}
// http://localhost:5000/api/OrderElements/add [body]
// http://localhost:5000/api/orderelements/update/${this.localItem.id} [body]
// http://localhost:5000/api/orderelements/delete/${this.localItem.id}

export const getOrderElementByOrderId = async (orderId) => {
    try {

    } catch (error) {
        console.error('Ошибка при получении данных (getOrderElementByOrderId): ', error);
        alert('Проблемы с сервером!');
    }
}

export const addOrderElement = async (orderElement) => {
    try {

    } catch (error) {
        console.error('Ошибка при добавлении данных (addOrderElement): ', error);
        alert('Проблемы с сервером!');
    }
}

export const updateOrderElement = async (orderElementId, newOrderElement) => {
    try {

    } catch (error) {
        console.error('Ошибка при обновлении данных (updateOrderElement): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteOrderElement = async (orderElementId) => {
    try {

    } catch (error) {
        console.error('Ошибка при удалении данных (deleteOrderElement): ', error);
        alert('Проблемы с сервером!');
    }
}