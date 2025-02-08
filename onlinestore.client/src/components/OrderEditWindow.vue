<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать заказ</h2>
            <hr>
            <div class="form">
                <label>Заказчик</label>
                <input type="text" v-model="localOrder.customerId">
            </div>
            <hr>
            <div class="form">
                <label>Дата заказа</label>
                <input type="text" v-model="localOrder.orderDate">
            </div>
            <hr>
            <div class="form">
                <label>Дата доставки</label>
                <input type="text" v-model="localOrder.shipmentDate">
            </div>
            <hr>
            <div class="form">
                <label>Статус</label>
                <select v-model="localOrder.orderStatus">
                    <option>new</option>
                    <option>in progress</option>
                    <option>completed</option>
                </select>
            </div>
    
            <div class="buttons">
                <button @click="deleteItem()">Удалить</button>
                <button v-if="role === '1'" @click="applyChanges()">Изменить</button>
                <button @click="cancelDialog()">Отмена</button>
            </div>
        </div>
    </div>
</template>

<script>
import ordersApi from '../api/ordersApi';

export default {
    props: {
        order: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            localOrder: { ...this.order },
            role: ''
        }
    },
    methods: {
        getRole() {
            this.role = localStorage.getItem('role');
        },
        makeOrderRequestBody() {
            return {
                customerId: this.localOrder.customerId,
                orderDate: this.localOrder.orderDate,
                shipmentDate: this.localOrder.shipmentDate,
                orderStatus: this.localOrder.orderStatus
            };
        },
        async applyChanges() {
            try {
                let newOrder = this.makeOrderRequestBody();
                const response = await ordersApi.updateOrder(this.localOrder.id, newOrder);
                if (!response) {
                    alert('Ошибка при изменении заказа (Order).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при изменении данных (Orders): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        async deleteItem() {
            try {
                const response = await ordersApi.delete(this.order.id);
                if (!response) {
                    alert('Ошибка при удалении заказа (Order).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (Orders): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        warnInfo(message, error) {
            console.error(message, error);
            alert('Проблемы с сервером!');
        },
        cancelDialog() {
            this.$emit('close-dialog', false);
        }
    },
    mounted() {
        this.getRole();
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

.form label {
    width: 50%;
    margin-right: 10px;
    padding: 10px;
}

.form input {
    width: 50%;
    padding: 10px;
    border-radius: 10px;
    border: 1px solid rgba(0,0,80,0.5);
}

h2 {
    color: #212933;
}

h2, label, button {
    font-family: "Sofia Sans", serif;
    font-optical-sizing: auto;
    font-style: normal;
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

.form {
    display: flex;
    margin-bottom: 15px;
}

hr {
    border: 1px dashed rgb(141, 169, 221, 0.5);
    margin: 15px 0px;
}
</style>