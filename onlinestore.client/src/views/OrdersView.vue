<script>
  import Pagination from '../components/PaginationComponent.vue'
  import OrderEditWindow from '@/components/OrderEditWindow.vue';
  import axios from 'axios';

  export default {
    name: 'Orders',
    components: { Pagination, OrderEditWindow },
    data() {
      return {
        currentPage: 1,
        totalPages: 1,
        pageSize: 24,
        totalCount: 0,
        myId: null,
        role: null,
        ordersUrl: '',
        records: [],
        selectedOrder: null,
        isOpenDialog: false
      }
    },
    methods: {
      getMyData() {
        this.myId = localStorage.getItem('guid');
        this.role = localStorage.getItem('role');
        
        this.makeUrl();
      },
      makeUrl() {
        if (this.role === '1') {
          this.ordersUrl = `http://localhost:5000/api/Orders/getpage?pageNumber=${this.currentPage}&pageSize=${this.pageSize}`;
        } else {
          this.ordersUrl = `http://localhost:5000/api/Orders/getbycustomer?id=${this.myId}&pageNumber=${this.currentPage}&pageSize=${this.pageSize}`;
        }
      },
      async getOrders(number = 1) {
        this.currentPage = number;
        this.makeUrl();
        try {
          const response = await axios.get(this.ordersUrl, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            this.records = response.data.orderResponses;
            this.totalCount = response.data.totalCount;
            this.totalPages = Math.ceil(this.totalCount / this.pageSize);
          } else {
            alert('Проблемы с сервером!');
            console.log("Статус ошибки: " + response.status);
          }
        } catch (error) {
          console.error('Ошибка при получении данных (Orders): ', error);
          alert('Проблемы с сервером!');
        }
      },
      async clickOnItem(index) {
        this.selectedOrder = this.records[index];
        this.clickWindowRedactor(true);
      },
      clickWindowRedactor(state) {
        this.isOpenDialog = state;
      }
    },
    mounted() {
      this.getMyData();
      this.getOrders();
    }
  }
</script>

<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <OrderEditWindow v-if="role === '1' && isOpenDialog === true" @close-dialog="clickWindowRedactor" :item="selectedOrder" />
  <div class="container">
    <h1 class="line">Заказы</h1>

    <div v-if="records.length > 0" class="table">
      <div class="row">
        <div class="cell colored">Заказ</div>
        <div class="cell colored">Заказчик</div>
        <div class="cell colored">Дата заказа</div>
        <div class="cell colored">Дата доставки</div>
        <div class="cell colored">Номер</div>
        <div class="cell colored">Статус</div>
        <div class="cell colored">Опции</div>
      </div>
      <div class="row" v-for="record in records" :key="record.id">
        <div class="cell base">{{ record.id }}</div>
        <div class="cell base">{{ record.customerName }} [ <style color="#9199a2">{{ record.customerId }}</style> ]</div>
        <div class="cell base">{{ record.orderDate }}</div>
        <div class="cell base">{{ record.shipmentDate }}</div>
        <div class="cell base">{{ record.orderNumber }}</div>
        <div class="cell base">{{ record.orderStatus }}</div>
        <div class="cell base"><a>Редактирование</a></div>
      </div>
    </div>
    <div v-else> <!---какой класс и стиль?-->
      <h2>Заказов нет.</h2>
    </div>

    <Pagination @page-changed="getOrders" :current="currentPage" :totalPages="totalPages" />

  </div>
  </template>

  <style scoped>
    .container {
      max-width: 65vw;
      margin: 0px auto;
      padding-top: 20px;
      background-color: #e0ebf7;
      filter: drop-shadow(0 0.2rem 0.25rem rgba(0, 0, 0, 0.2));
    }

    .table {
      display: table;
      width: 100%;
      border-collapse: collapse;
    }

    .row {
      display: table-row;
    }

    .cell {
      display: table-cell;
      border: 1px solid #6678b1;
      padding: 10px;
      text-align: left;
      font-size: 18px;
      line-height: 28px;
    }

    .colored {
      font-weight: bold;
      background-color: rgba(163, 194, 232, 1);
    }

    .base {
      background-color: white;
    }

    a, h1, h2, h3, p, .cell {
      font-family: "Sofia Sans", serif;
      font-optical-sizing: auto;
      font-weight: 500;
      font-style: normal;
      color: #1c2633;
    }

    h1 {
      font-size: 26px;
      line-height: 36px;
      padding: 20px 10px;
    }

    h2 {
      font-size: 24px;
      line-height: 32px;
      padding: 20px 10px;
    }

    h3 {
      font-size: 22px;
      line-height: 30px;
      padding: 15px 10px;
    }

    p {
      font-size: 18px;
      line-height: 28px;
      padding: 0px 10px;
    }

    .line {
      background-image: linear-gradient(rgba(255, 255, 255, 0.1), rgba(255, 255, 255, 0.9), rgba(255, 255, 255, 0.1));
    }
  </style>
