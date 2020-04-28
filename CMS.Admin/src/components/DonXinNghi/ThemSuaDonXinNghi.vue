
<template>
    <v-dialog v-model="isShow" width="600" scrollable persistent>
        <v-card>
            <v-card-title class="teal lighten-2 white--text pa-2">
                <span class="title">{{ isUpdate? 'Cập nhập đơn xin nghỉ' : 'Thêm mới đơn xin nghỉ' }}</span>
                <v-spacer></v-spacer>
                <v-btn class="white--text ma-0" small icon @click="hide()">
                    <v-icon>close</v-icon>
                </v-btn>
            </v-card-title>
            <v-card-text>
                <v-form scope="frmAddEdit">
                    <v-layout row wrap>
                        <v-flex xs12 md6>
                            <v-datetimepicker v-model="leave_Request.TimeStart"
                                              label="Thời gian ra"
                                              :error-messages="errors.collect('Thời gian ra', ' frmAddEdit')"
                                              v-validate="'required'" @input="updateTimeEnd()"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Thời gian vào"
                                              hide-details
                                              clearable></v-datetimepicker>
                        </v-flex>
                        <v-flex xs12 md6>
                            <v-datetimepicker v-model="leave_Request.TimeEnd"
                                              label="Thời gian vào"
                                              :error-messages="errors.collect('Thời gian vào', ' frmAddEdit')"
                                              v-validate="'required'"
                                              data-vv-scope="frmAddEdit"
                                              data-vv-name="Thời gian vào"
                                              hide-details
                                              clearable></v-datetimepicker>
                        </v-flex>
                        <v-flex xs12>
                            <v-textarea v-model="leave_Request.Reason"
                                        counter="500"
                                        auto-grow
                                        label="Lý do"
                                        type="text"
                                        :error-messages="errors.collect('Lý do', 'frmAddEdit')"
                                        v-validate="''"
                                        data-vv-scope="frmAddEdit"
                                        clearable
                                        data-vv-name="Lý do"></v-textarea>
                        </v-flex>

                    </v-layout>
                </v-form>
            </v-card-text>
            <v-card-actions>
                <v-spacer></v-spacer>
                <v-btn text @click.native="hide()">Hủy</v-btn>
                <v-btn text @click.native="save" color="teal lighten-2"
                       :loading="loadingSave">Lưu</v-btn>
            </v-card-actions>
        </v-card>
    </v-dialog>
</template>
<script lang="ts">
    import { Vue } from 'vue-property-decorator';
    import VChooseFile from '@/components/Commons/VChooseFile.vue';
    import Leave_RequestApi, { Leave_RequestApiSearchParams } from '@/apiResources/Leave_RequestApi';
    import { Leave_Request } from '@/models/Leave_Request';
    import { Users } from '@/models/Users';
import UsersApi from '../../apiResources/UsersApi';
    export default Vue.extend({
        $_veeValidate: {
            validator: 'new'
        },
        components: {VChooseFile},
        data() {
            return {
                isUpdate: false,
                dsNhanVien: [] as Users[],
                leave_Request: {} as Leave_Request,
                loading: false,
                searchParamsLeave_Request: {} as Leave_RequestApiSearchParams,
                editing: null as any,
                search: null,
                index: -1,
                isShow: false
            }
        },
        watch: {
        },
        mounted() {
            this.getDSNhanVien();
        },
        computed: {
        },
        methods: {
            updateTimeEnd() {
                this.leave_Request.TimeEnd = this.$moment(this.leave_Request.TimeStart).add(15, 'minutes')

            },
            show(isUpdate: boolean, item: any) {
                this.isShow = true;
                this.isUpdate = isUpdate;
                if (isUpdate) {
                    this.leave_Request.Id = item.Id;
                    this.getDataFromApi(this.leave_Request.Id);
                }
                else {
                    this.leave_Request = {} as Leave_Request;
                }
            },
            getDataFromApi(id: number): void {
                Leave_RequestApi.detail(id).then(res => {
                    this.leave_Request = res;
                });
            },
            getDSNhanVien(): void {
                UsersApi.search({ rowsPerPage: -1 }).then(res => {
                    this.dsNhanVien = res.Data;
                });
            },
            hide() {
                this.isShow = false
            },
            save(): void {
                this.$validator.validateAll('frmAddEdit').then((res) => {
                    if (res) {
                        this.leave_Request.User_Approve = undefined;
                        this.leave_Request.User_Leave = undefined;
                        this.leave_Request.User_LeaveID = this.$store.state.user.Profile.UserId
                        if (this.isUpdate) {
                            this.loading = true;
                            Leave_RequestApi.update(this.leave_Request.Id, this.leave_Request).then(res => {
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Cập nhật thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Cập nhật thất bại!');
                            });
                        } else {
                            this.loading = true;
                            Leave_RequestApi.insert(this.leave_Request).then(res => {
                                this.leave_Request = res;
                                this.isUpdate = true;
                                this.loading = false;
                                this.isShow = false;
                                this.$emit("save");
                                this.$snotify.success('Thêm mới thành công!');
                            }).catch(res => {
                                this.loading = false;
                                this.$snotify.error('Thêm mới thất bại!');
                            });
                        }
                    }
                });
            },
        }
    });
</script>

