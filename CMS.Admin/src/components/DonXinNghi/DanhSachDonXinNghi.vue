<template>
    <v-flex xs12>
        <v-breadcrumbs divider="/" class="pa-0">
            <v-icon slot="divider">chevron_right</v-icon>
            <v-breadcrumbs-item>
                <v-btn flat class="ma-0" @click="$router.go(-1)" small><v-icon>arrow_back</v-icon> Quay lại</v-btn>
            </v-breadcrumbs-item>
            <v-breadcrumbs-item to="/donxinnghi" exact>Danh sách xin nghỉ</v-breadcrumbs-item>
        </v-breadcrumbs>
        <v-card>
            <v-card-text>
                <v-layout row wrap>
                    <v-flex xs12 sm6 md3>
                        <v-text-field label="Tìm kiếm..." v-model="searchParamsLeave_Request.keywords" @input="getDataFromApi(searchParamsLeave_Request)" single-line hide-details></v-text-field>
                    </v-flex>
                    <v-spacer></v-spacer>
                    <v-btn color="info" @click="showModalAddEdit(false, {})" class="mt-3" small>Thêm mới</v-btn>
                    <v-flex xs12>
                        <v-data-table :headers="tableHeader"
                                      :items="dsLeave_Request"
                                      @update:pagination="getDataFromApi" :pagination.sync="searchParamsLeave_Request"
                                      :total-items="searchParamsLeave_Request.totalItems"
                                      :loading="loadingTable"
                                      class="table-border table">
                            <template slot="items" slot-scope="props">
                                <td class="text-xs-center">{{ props.index + 1 }}</td>
                                <td class="text-xs-center">{{ props.item.User_Leave ? props.item.User_Leave.UserName : "" }}</td>
                                <td class="text-xs-center">{{ props.item.TimeStart | moment("DD/MM/YYYY") }}</td>
                                <td class="text-xs-center">{{ props.item.TimeStart | moment("hh:mm") }} - {{ props.item.TimeEnd | moment("hh:mm") }}</td>
                                <td class="text-xs-center">{{ props.item.Reason }}</td>
                                <td class="text-xs-center">{{ props.item.User_Approve ? props.item.User_Approve.UserName : ""}}</td>
                                <td class="text-xs-center">
                                    <span v-if="laNguoiPheDuyet == true">
                                        <v-btn flat small @click="duyetDon(props.item)" class="ma-0">
                                            Duyệt
                                        </v-btn>
                                    </span>
                                    <span v-else>
                                        {{ props.item.User_ApproveID != null? "Đã duyệt" : "Chưa duyệt" }}
                                    </span>
                                </td>
                                <td class="text-xs-center">
                                    <v-btn flat icon small @click="showModalAddEdit(true, props.item)" class="ma-0">
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
        <them-sua-don-xin-nghi ref="themSuaDonXinNghi" @save='getDataFromApi(searchParamsLeave_Request)'></them-sua-don-xin-nghi>

        <v-dialog v-model="dialogConfirmDelete" max-width="290">
            <v-card>
                <v-card-title class="headline">Xác nhận xóa</v-card-title>
                <v-card-text class="pt-3">Bạn có chắc chắn muốn xóa?</v-card-text>
                <v-card-actions>
                    <v-spacer></v-spacer>
                    <v-btn @click.native="dialogConfirmDelete=false" flat>Hủy</v-btn>
                    <v-btn color="red darken-1" @click.native="deleteLeave_Request" flat>Xác Nhận</v-btn>
                </v-card-actions>
            </v-card>
        </v-dialog>
    </v-flex>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import Leave_RequestApi, { Leave_RequestApiSearchParams } from '@/apiResources/Leave_RequestApi';
    import { Leave_Request } from '@/models/Leave_Request';
    import ThemSuaDonXinNghi from './ThemSuaDonXinNghi.vue';

    export default Vue.extend({
        components: {
            ThemSuaDonXinNghi
        },
        data() {
            return {
                dsLeave_Request: [] as Leave_Request[],
                tableHeader: [
                    { text: '#', value: '#', align: 'center', sortable: true },
                    { text: 'Tên người xin nghỉ', value: 'ThuTu', align: 'center', sortable: true },
                    { text: 'Ngày xin nghỉ', value: 'HienThi', align: 'center', sortable: true },
                    { text: 'Thời gian', value: 'HienThi', align: 'center', sortable: true },
                    { text: 'Lý do', value: 'HienThi', align: 'center', sortable: true },
                    { text: 'Người phê duyệt', value: 'HienThi', align: 'center', sortable: true },
                    { text: 'Trạng thái', value: 'HienThi', align: 'center', sortable: true },
                    { text: 'Thao tác', value: '#', align: 'center', sortable: false },
                ],
                searchParamsLeave_Request: { includeEntities: true, rowsPerPage: 10 } as Leave_RequestApiSearchParams,
                loadingTable: false,
                selectedLeave_Request: {} as Leave_Request,
                dialogConfirmDelete: false,
                laNguoiPheDuyet: false
            }
        },
        watch: {
        },
        created() {
            this.getDataFromApi(this.searchParamsLeave_Request);
            if (this.$store.state.user.Profile.LoaiTaiKhoanID == 4) // admin
                this.laNguoiPheDuyet = true
        },
        methods: {
            getDataFromApi(searchParamsLeave_Request: Leave_RequestApiSearchParams): void {
                Leave_RequestApi.search(searchParamsLeave_Request).then(res => {
                    this.dsLeave_Request = res.Data;
                });
            },
            showModalAddEdit(isUpdate: boolean, item: any){
                (this.$refs.themSuaDonXinNghi as any).show(isUpdate, item);
            },
            duyetDon(item: any) {
                item.User_ApproveID = this.$store.state.user.Profile.UserId
                item.User_Approve = undefined;
                item.User_Leave = undefined;
                Leave_RequestApi.update(item.Id, item).then(res => {
                    this.getDataFromApi(this.searchParamsLeave_Request)
                    this.$snotify.success('Duyệt đơn thành công!');
                }).catch(res => {
                    this.$snotify.error('Duyệt đơn thất bại!');
                });
            },
            confirmDelete(leaveRequest: Leave_Request): void {
                this.selectedLeave_Request = leaveRequest;
                this.dialogConfirmDelete = true;
            },
            deleteLeave_Request(): void {
                Leave_RequestApi.delete(this.selectedLeave_Request.Id).then(res => {
                    this.$snotify.success('Xóa thành công!');
                    this.getDataFromApi(this.searchParamsLeave_Request);
                    this.dialogConfirmDelete = false;
                }).catch(res => {
                    this.$snotify.error('Xóa thất bại!');
                });
            },
        }
    });
</script>

