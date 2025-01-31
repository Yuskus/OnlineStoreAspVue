<script>
  import Pagination from '../components/PaginationComponent.vue'
  import axios from 'axios';

  export default {
    name: 'Orders',
    components: { Pagination },
    data() {
      return {
        currentPage: 1,
        totalPages: 1,
        pageSize: 24,
        myId: null,
        records: []
      }
    },
    methods: {
      async getOrders() {
        this.myId = '';
        // написать запрос получения GUID кастомера
        
        let url = locatStorage.getItem('role') === 'Manager' ? 'http://localhost:5000/api/Orders/getall' : `http://localhost:5000/api/Orders/getbyid/${this.myId}`;
        
        try {
          const response = await axios.get(url, {
            headers: {
              'authorization': `Bearer ${localStorage.getItem('jwt')}`
            }
          });
          
          if (response.status === 200 && response.data) {
            this.records = response.data;
          } else {
            alert('Проблемы с сервером!');
            console.log("Статус ошибки: " + response.status);
          }
        } catch (error) {
          console.error('Ошибка при получении данных (Orders): ', error);
          alert('Проблемы с сервером!');
        }
      }
    },
    mounted() {
      this.getOrders();
    }
  }
</script>

<template>
  <link href="https://fonts.googleapis.com/css2?family=Noto+Sans:ital,wght@0,100..900;1,100..900&family=Sofia+Sans:ital,wght@0,1..1000;1,1..1000&display=swap" rel="stylesheet">
  <div class="container">
    <h1 class="line">Заказы</h1>

    <div class="table">
      <div class="row colored">
        <div class="cell">ID Заказа</div>
        <div class="cell">ID Заказчика</div>
        <div class="cell">Дата от</div>
        <div class="cell">Дата до</div>
        <div class="cell">Номер</div>
        <div class="cell">Статус</div>
        <div class="cell">Опции</div>
      </div>
      <div class="row" v-for="record in records" :key="record.id">
        <div class="cell">{{ record.id }}</div>
        <div class="cell">{{ record.customerId }}</div>
        <div class="cell">{{ record.orderDate }}</div>
        <div class="cell">{{ record.shipmentDate }}</div>
        <div class="cell">{{ record.orderNumber }}</div>
        <div class="cell">{{ record.orderStatus }}</div>
        <div class="cell"><a>Редактирование</a></div>
      </div>
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
      background-color: white;
    }

    .colored {
      font-weight: bold;
      background-color: rgba(163, 194, 232, 1);
    }

    .base {
      background-color: white;
    }

    a, h1, h3, p, .cell {
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
