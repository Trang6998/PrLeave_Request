<template>
    <v-flex xs12>
        <v-breadcrumbs divider="/" class="pa-0">
            <v-icon slot="divider">chevron_right</v-icon>
            <v-breadcrumbs-item>
                <v-btn flat class="ma-0" @click="$router.go(-1)" small><v-icon>arrow_back</v-icon> Quay lại</v-btn>
            </v-breadcrumbs-item>
            <v-breadcrumbs-item to="/luottruycap" exact>LuotTruyCap</v-breadcrumbs-item>
        </v-breadcrumbs>
        <v-card>
            <v-card-text>
                <v-layout row wrap>
                    <v-flex xs12 sm6 md3>
                        <v-text-field label="Tìm kiếm..." v-model="searchParamsLuotTruyCap.q" @input="getDataFromApi" single-line></v-text-field>
                    </v-flex>
                    <v-flex xs12 sm6 md3>
                        <v-text-field @input="getDataFromApi" v-model="searchParamsLuotTruyCap.fromThoiGian" label="From Thời gian truy cập" placeholder="From Thời gian truy cập" type="datetime-local"></v-text-field>
                    </v-flex>
                    <v-flex xs12 sm6 md3>
                        <v-text-field @input="getDataFromApi" v-model="searchParamsLuotTruyCap.toThoiGian" label="To Thời gian truy cập" placeholder="To Thời gian truy cập" type="datetime-local"></v-text-field>
                    </v-flex>
                    <v-btn color="info" to="/luottruycap/add" small>Thêm mới</v-btn>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsLuotTruyCap"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsLuotTruyCap"
                                      :total-items="searchParamsLuotTruyCap.totalItems"
                                      :loading="loadingTable"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td>{{ props.item.IP }}</td>
                                <td>{{ props.item.ThoiGian === null ? "" : props.item.ThoiGian|moment('DD/MM/YYYY HH:mm:ss') }}</td>
                                <td>{{ props.item.ThietBi }}</td>
                                <td>{{ props.item.TrangTruyCapDauTien }}</td>
                                <td class="text-xs-center">
                                    <v-btn flat icon small :to="'/luottruycap/'+props.item.LuotTruyCapId" class="ma-0">
                                        <v-icon small>edit</v-icon>
                                    </v-btn>
                                    <v-btn flat color="red" icon small class="ma-0" @click="confirmDelete(props.item)">
                                        <v-icon small>delete</v-icon>
                                    </v-btn>
                                </td>
                            </template>
                        </v-data-table>
                    </v-flex>
                </v-layout>
            </v-card-text>
        </v-card>
        <v-dialog v-model="dialogConfirmDelete" max-width="290">
            <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteLuotTruyCap" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import LuotTruyCapApi, { LuotTruyCapApiSearchParams } from '@/apiResources/LuotTruyCapApi';
    import { LuotTruyCap } from '@/models/LuotTruyCap';

    export default Vue.extend({
        components: {},
        data() {
            return {
                dsLuotTruyCap: [] as LuotTruyCap[],
                tableHeader: [
                    { text: 'IP', value: 'IP', align: 'center', sortable: true },
                    { text: 'Thời gian truy cập', value: 'ThoiGian', align: 'center', sortable: true },
                    { text: 'Thiết bị', value: 'ThietBi', align: 'center', sortable: true },
                    { text: 'Trang truy cập đầu tiên', value: 'TrangTruyCapDauTien', align: 'center', sortable: true },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsLuotTruyCap: { includeEntities: true, rowsPerPage: 10 } as LuotTruyCapApiSearchParams,
                loadingTable: false,
                selectedLuotTruyCap: {} as LuotTruyCap,
                dialogConfirmDelete: false,
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi();
        },
        methods: {
            getDataFromApi(): void {
                LuotTruyCapApi.search(this.searchParamsLuotTruyCap).then(res => {
                    this.dsLuotTruyCap = res.Data;
                });
            },
            confirmDelete(luotTruyCap: LuotTruyCap): void {
                this.selectedLuotTruyCap = luotTruyCap;
                this.dialogConfirmDelete = true;
            },
            deleteLuotTruyCap(): void {
                LuotTruyCapApi.delete(this.selectedLuotTruyCap.LuotTruyCapId).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi();
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>


