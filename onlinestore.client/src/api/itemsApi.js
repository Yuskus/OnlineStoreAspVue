import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Items/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Items/getpagebycategory/${category}?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Items/getcategories
// http://localhost:5000/api/items/add [body]
// http://localhost:5000/api/items/update/${this.localItem.id} [body]
// http://localhost:5000/api/items/delete/${this.localItem.id}

export const getPageOfItems = async (pageNumber, pageSize) => {
    try {

    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfItems): ', error);
        alert('Проблемы с сервером!');
    }
}

export const getPageOfItemsByCategory = async (category, pageNumber, pageSize) => {
    try {

    } catch (error) {
        console.error('Ошибка при получении данных (getPageOfItemsByCategory): ', error);
        alert('Проблемы с сервером!');
    }
}

export const getCategories = async () => {
    try {

    } catch (error) {
        console.error('Ошибка при получении данных (getCategories): ', error);
        alert('Проблемы с сервером!');
    }
}

export const addItem = async (item) => {
    try {

    } catch (error) {
        console.error('Ошибка при добавлении данных (addItem): ', error);
        alert('Проблемы с сервером!');
    }
}

export const updateItem = async (itemId, newItem) => {
    try {

    } catch (error) {
        console.error('Ошибка при обновлении данных (updateItem): ', error);
        alert('Проблемы с сервером!');
    }
}

export const deleteItem = async (itemId) => {
    try {

    } catch (error) {
        console.error('Ошибка при удалении данных (deleteItem): ', error);
        alert('Проблемы с сервером!');
    }
}