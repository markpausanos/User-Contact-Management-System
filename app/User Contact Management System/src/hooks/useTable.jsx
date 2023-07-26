import { useState } from "react";
import {
	Table,
	TableHead,
	TableRow,
	TableCell,
	TablePagination,
	TableSortLabel,
} from "@mui/material";

const useTable = ({ contacts, headCells, filterFn }) => {
	const pages = [5, 10, 25];
	const [page, setPage] = useState(0);
	const [rowsPerPage, setRowsPerPage] = useState(pages[page]);
	const [order, setOrder] = useState();
	const [orderBy, setOrderBy] = useState();

	if (!contacts) {
		return;
	}

	const TblContainer = (props) => <Table>{props.children}</Table>;

	const TblHead = (props) => {
		const handleSortRequest = (cellId) => {
			const isAsc = orderBy === cellId && order === "asc";
			setOrder(isAsc ? "desc" : "asc");
			setOrderBy(cellId);
		};

		return (
			<TableHead>
				<TableRow>
					{headCells.map((headCell) => (
						<TableCell
							sx={{ textAlign: "center" }}
							key={headCell.id}
							sortDirection={orderBy === headCell.id ? order : false}
						>
							{headCell.disableSorting ? (
								headCell.label
							) : (
								<TableSortLabel
									active={orderBy === headCell.id}
									direction={orderBy === headCell.id ? order : "asc"}
									onClick={() => {
										handleSortRequest(headCell.id);
									}}
								>
									{headCell.label}
								</TableSortLabel>
							)}
						</TableCell>
					))}
				</TableRow>
			</TableHead>
		);
	};

	const handleChangePage = (event, newPage) => {
		setPage(newPage);
	};

	const handleChangeRowsPerPage = (event) => {
		setRowsPerPage(parseInt(event.target.value, 10));
		setPage(0);
	};

	const TblPagination = () => (
		<TablePagination
			component="div"
			page={page}
			rowsPerPageOptions={pages}
			rowsPerPage={rowsPerPage}
			count={contacts.length}
			onChangePage={handleChangePage}
			onChangeRowsPerPage={handleChangeRowsPerPage}
		/>
	);

	function stableSort(array, comparator) {
		const stabilizedThis = array.map((el, index) => [el, index]);
		stabilizedThis.sort((a, b) => {
			const order = comparator(a[0], b[0]);
			if (order !== 0) return order;
			return a[1] - b[1];
		});
		return stabilizedThis.map((el) => el[0]);
	}

	function getComparator(order, orderBy) {
		return order === "desc"
			? (a, b) => descendingComparator(a, b, orderBy)
			: (a, b) => -descendingComparator(a, b, orderBy);
	}

	function descendingComparator(a, b, orderBy) {
		if (b[orderBy] < a[orderBy]) {
			return -1;
		}
		if (b[orderBy] > a[orderBy]) {
			return 1;
		}
		return 0;
	}

	const contactsAfterPagingAndSorting = () => {
		return stableSort(
			filterFn.fn(contacts),
			getComparator(order, orderBy)
		).slice(page * rowsPerPage, (page + 1) * rowsPerPage);
	};

	return {
		TblContainer,
		TblHead,
		TblPagination,
		contactsAfterPagingAndSorting,
	};
};

export default useTable;
