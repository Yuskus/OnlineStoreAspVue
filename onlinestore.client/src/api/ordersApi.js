import axios from 'axios';

const API_URL = 'http://localhost:5000';

// http://localhost:5000/api/Orders/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Orders/getpagebycustomer/{this.myId}?pageNumber=${this.currentPage}&pageSize=${this.pageSize}
// http://localhost:5000/api/Orders/getbasket/${this.myId}
// http://localhost:5000/api/orders/placeanorder/${this.basketOrder.id} []
// http://localhost:5000/api/orders/update/${this.localOrder.id} [body]
// http://localhost:5000/api/orders/delete/${this.order.id}

export const getPageOfOrders = (pageNumber, pageSize) => {

}

export const getPageOfOrdersByCustomer = (customerId, pageNumber, pageSize) => {
    
}

export const getBasket = (customerId) => {

}

export const placeAnOrder = (orderId) => {

}

export const updateOrder = (orderId) => {

}

export const deleteOrder = (orderId) => {

}