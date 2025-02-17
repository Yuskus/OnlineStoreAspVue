<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать список товаров</h2>
            
            <BasketComponent :basket="basket" @refresh-basket="getBasket" />
    
            <div class="buttons">
                <button @click="deleteThisOrder()">Удалить заказ</button>
                <button @click="cancelDialog()">Назад</button>
            </div>
        </div>
    </div>
</template>

<script>
import { deleteOrder } from '../../api/ordersApi';
import { getOrderElementByOrderId } from '../../api/orderElementsApi';

import BasketComponent from '../elements/BasketComponent.vue';

export default {
    components: { BasketComponent },
    props: {
        order: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            basket: []
        }
    },
    methods: {
        async deleteThisOrder() {
            try {
                const response = await deleteOrder(this.order.id);
                if (!response) {
                    alert('Ошибка при очистке корзины.');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (BasketView): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async getBasket() {
            try {
                const response = await getOrderElementByOrderId(this.order.id);
                if (response) {
                    this.basket = response;
                } else {
                    alert('Ошибка при получении содержимого корзины.');
                }
            } catch (error) {
                this.warnInfo('Ошибка при получении данных (BasketView): ', error);
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert(error.message);
        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    },
    mounted() {
        this.getBasket();
    }
}
</script>

<style scoped>
.dialog-over {
    position: fixed;
    display: flex;
    justify-content: center;
    align-items: center;
    top: 0px;
    bottom: 0px;
    left: 0px;
    right: 0px;
    transform: scale(1.25);
    background-color: rgba(0,0,0,0.4);
    z-index: 1000;
}

.dialog {
    background-color: white;
    padding: 20px;
    border-radius: 5px;
    box-shadow: 0 2px 10px rgba(0,0,0,0.3);
    min-width: 400px;
    width: 50vw;
    min-height: fit-content;
}

h2 {
    color: #212933;
    margin-bottom: 20px;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

.buttons button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

.buttons button:hover {
    background-color: rgba(0,0,30,0.25);
}

.buttons button:focus {
    background-color: rgba(0,0,30,0.4);
}
</style>