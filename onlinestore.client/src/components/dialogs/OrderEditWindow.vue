<template>
    <div class="dialog-over">
        <div class="dialog">
            <h2>Редактировать заказ</h2>
            <div v-if="isEditOrderContent">
                <BasketComponent :basket="basket" @refresh-basket="getOrderElements" />
            </div>
            <div v-else>
                <DialogLineForm v-model="localOrder.customerId" labelName="Заказчик" inputType="text" placeholderText="6F9619FF-8B86-D011-B42D-00CF4FC964FF" />
                <DialogLineForm v-model="localOrder.orderDate" labelName="Дата заказа" inputType="text" placeholderText="YYYY-MM-DD" />
                <DialogLineForm v-model="localOrder.shipmentDate" labelName="Дата доставки" inputType="text" placeholderText="YYYY-MM-DD" />

                <DialogOptionsForm v-model="localOrder.orderStatus" labelName="Статус" :options="options" />
            </div>

            <div class="buttons">
                <button @click="switchEditFormat()">{{ switcherButtonText }}</button>
                <button @click="applyChanges()">Изменить</button>
                <button @click="deleteItem()">Удалить</button>
                <button @click="cancelDialog()">Назад</button>
            </div>
        </div>
    </div>
</template>

<script>
import { updateOrder, deleteOrder } from '../../api/ordersApi';
import { getOrderElementByOrderId } from '../../api/orderElementsApi';

import DialogLineForm from '../forms/DialogLineForm.vue';
import DialogOptionsForm from '../forms/DialogOptionsForm.vue';
import BasketComponent from '../elements/BasketComponent.vue';

export default {
    components: { DialogLineForm, DialogOptionsForm, BasketComponent },
    props: {
        order: {
            type: Object,
            required: true
        }
    },
    data() {
        return {
            localOrder: { ...this.order },
            options: ['basket', 'new', 'in progress', 'completed'],
            role: '',
            basket: [],
            isEditOrderContent: false,
            switcherButtonText: 'Редактировать содержимое'
        }
    },
    methods: {
        getRole() {
            this.role = localStorage.getItem('role');
        },
        async getOrderElements() {
            try {
                const response = await getOrderElementByOrderId(this.order.id);
                if (response) {
                    this.basket = response;
                } else {
                    alert('Ошибка при получении содержимого заказа.');
                }
            } catch (error) {
                this.warnInfo('Ошибка при получении данных (OrderView): ', error);
            }
        },
        async applyChanges() {
            try {
                const response = await updateOrder(this.localOrder.id, this.localOrder);
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
                const response = await deleteOrder(this.order.id);
                if (!response) {
                    alert('Ошибка при удалении заказа (Order).');
                }
            } catch (error) {
                this.warnInfo('Ошибка при удалении данных (Orders): ', error);
            } finally {
                this.cancelDialog();
            }
        },
        switchEditFormat() {
            if (this.isEditOrderContent) {
                this.switcherButtonText = 'Редактировать содержимое';
                this.isEditOrderContent = false;
            } else {
                this.switcherButtonText = 'Редактировать информацию';
                this.isEditOrderContent = true;
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
        this.getRole();
        this.getOrderElements();
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
    padding-bottom: 20px;
}

.buttons {
    width: fit-content;
    margin-top: 20px;
    right: 0px;
    float: right;
}

button {
    margin: 0 10px;
    padding: 10px;
    border-radius: 10px;
    border: none;
    background-color: rgba(0,0,30,0.1);
}

button:hover {
    background-color: rgba(0,0,30,0.25);
}

button:active {
    background-color: rgba(0,0,30,0.4);
}
</style>