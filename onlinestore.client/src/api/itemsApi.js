import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Items/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Items/getpagebycategory/${category}?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Items/getcategories
// http://localhost:5000/api/items/add [body]
// http://localhost:5000/api/items/update/${this.localItem.id} [body]
// http://localhost:5000/api/items/delete/${this.localItem.id}

export const getPageOfItems = (pageNumber, pageSize) => {

}

export const getPageOfItemsByCategory = (category, pageNumber, pageSize) => {

}

export const getCategories = () => {

}

export const addItem = (item) => {

}

export const updateItem = (itemId, newItem) => {

}

export const deleteItem = (itemId) => {
    
}